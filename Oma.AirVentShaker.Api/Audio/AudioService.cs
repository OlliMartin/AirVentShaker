using Microsoft.Extensions.Options;
using Oma.AirVentShaker.Api.Interfaces;
using Oma.AirVentShaker.Api.Model;
using Oma.AirVentShaker.Api.Model.Settings;
using SoundFlow.Abstracts;
using SoundFlow.Abstracts.Devices;
using SoundFlow.Backends.MiniAudio;
using SoundFlow.Components;
using SoundFlow.Enums;
using SoundFlow.Structs;

namespace Oma.AirVentShaker.Api.Audio;

public sealed class AudioService : IAudioService, IDisposable
{
  private readonly AudioEngine _audioEngine;
  private readonly SemaphoreSlim _mutex = new(initialCount: 1);
  private readonly Oscillator _oscillator;
  private readonly AudioPlaybackDevice _playbackDevice;

  private CancellationTokenSource? _delayCancellationTokenSource;

  public AudioService(ILogger<AudioService> logger, IOptions<AudioSettings> audioOptions)
  {
    _audioEngine = new MiniAudioEngine();
    AudioFormat audioFormat = AudioFormat.Cd;

    DeviceInfo[] availableDevices = _audioEngine
      .PlaybackDevices;
    
    logger.LogInformation("Available playback devices:");
    foreach(var device in availableDevices)
    {
      Console.WriteLine("\tDevice: {0}, IsDefault: {1}", device.Name, device.IsDefault);
      
      logger.LogInformation(
        "\tDevice: {DeviceName}, IsDefault: {IsDefault}",
        device.Name,
        device.IsDefault);
    }
    
    string playbackDeviceName = audioOptions.Value.PlaybackDevice;
    List<DeviceInfo> matchedDevice = availableDevices
      .Where(d => d.Name == playbackDeviceName)
      .ToList();

    if (matchedDevice.Count != 1)
    {
      throw new InvalidOperationException($"Could not locate specified device '{playbackDeviceName}'. Cannot proceed.");
    }

    DeviceInfo selectedDevice = matchedDevice.Single();
    
    logger.LogInformation("Initializing device {DeviceName} with id: {DeviceId}.", selectedDevice.Name, selectedDevice.Id);
    
    _playbackDevice = _audioEngine.InitializePlaybackDevice(selectedDevice, audioFormat);
    
    logger.LogInformation("Creating oscillator.");
    
    _oscillator = new Oscillator(_audioEngine, audioFormat)
      { Frequency = 50, Type = Oscillator.WaveformType.Sine, Amplitude = 0.2f, Enabled = false, };
    
    logger.LogInformation("Adding oscillator to master mix.");
    
    _playbackDevice.MasterMixer.AddComponent(_oscillator);
    
    logger.LogInformation("Starting playback device.");
    _playbackDevice.Start();
  }

  public async Task PlayAsync(
    IWaveDescriptor waveDescriptor,
    TimeSpan duration,
    CancellationToken cancelToken
  )
  {
    try
    {
      await _mutex.WaitAsync(cancelToken);

      if (_delayCancellationTokenSource is not null)
      {
        await _delayCancellationTokenSource.CancelAsync();
      }

      _delayCancellationTokenSource = new CancellationTokenSource();

      AdjustOscillator(waveDescriptor);
      ScheduleStop(duration, _delayCancellationTokenSource.Token);

      cancelToken.Register(() => _oscillator.Enabled = false);
    }
    finally
    {
      _mutex.Release();
    }
  }

  public async Task StopAsync(CancellationToken cancelToken)
  {
    try
    {
      await _mutex.WaitAsync(cancelToken);

      _oscillator.Enabled = false;

      await (_delayCancellationTokenSource?.CancelAsync() ?? Task.CompletedTask);
      _delayCancellationTokenSource?.Dispose();
      _delayCancellationTokenSource = null;
    }
    finally
    {
      _mutex.Release();
    }
  }

  public async Task UpdateAmplitudeAsync(float newVal, CancellationToken cancelToken)
  {
    try
    {
      await _mutex.WaitAsync(cancelToken);

      _oscillator.Amplitude = newVal;
    }
    finally
    {
      _mutex.Release();
    }
  }

  public void Dispose()
  {
    _playbackDevice.Stop();
    
    _delayCancellationTokenSource?.Dispose();
    _playbackDevice.Dispose();
    _audioEngine.Dispose();
  }

  private void ScheduleStop(TimeSpan duration, CancellationToken cancelToken)
  {
    Task.Run(
      async () =>
      {
        try
        {
          await Task.Delay(duration, cancelToken);
          _oscillator.Enabled = false;

          _delayCancellationTokenSource?.Dispose();
          _delayCancellationTokenSource = null;
        }
        catch (OperationCanceledException)
        {
          // do nothing, empty on purpose (PlayAsync was called again)
        }
      }
    );
  }

  private void AdjustOscillator(IWaveDescriptor waveDescriptor)
  {
    _playbackDevice.MasterMixer.RemoveComponent(_oscillator);

    if (waveDescriptor is SineWaveDescriptor sineDescriptor)
    {
      _oscillator.Type = Oscillator.WaveformType.Sine;
      _oscillator.Frequency = sineDescriptor.Frequency;
    }
    else
    {
      throw new InvalidOperationException(
        $"Unknown wave descriptor {waveDescriptor.GetType().Name}. This is a programming error."
      );
    }

    _oscillator.Amplitude = waveDescriptor.Amplitude;

    _oscillator.Enabled = true;
    _playbackDevice.MasterMixer.AddComponent(_oscillator);
  }
}