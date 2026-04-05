using System.Device.Spi;
using System.Numerics;
using Iot.Device.Adxl345;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Oma.AirVentShaker.Api.Interfaces;
using Oma.AirVentShaker.Api.Model;
using Oma.AirVentShaker.Api.Model.Settings;

namespace Oma.AirVentShaker.Api.Sensors;

public sealed class Adxl345SensorService : ISensorService, IDisposable
{
  private readonly SpiDevice _device;
  private readonly GlobalState _globalState;
  private readonly ILogger<Adxl345SensorService> _logger;
  private readonly Adxl345 _sensor;
  private readonly IOptions<SensorSettings> _sensorOptions;

  private Vector3? _baselineAcc = null;

  public Adxl345SensorService(
    ILogger<Adxl345SensorService> logger,
    IOptions<SensorSettings> sensorOptions,
    GlobalState globalState
  )
  {
    _logger = logger;
    _sensorOptions = sensorOptions;
    _globalState = globalState;

    IoConfiguration ioSettings = _sensorOptions.Value.Io;
    AccelerometerConfiguration accelerometerConfiguration = ioSettings.Accelerometer;

    _logger.LogInformation(
      "Opening accelerometer at bus {bus}, line {line}",
      accelerometerConfiguration.BusId,
      accelerometerConfiguration.Line
    );

    SpiConnectionSettings settings = new(accelerometerConfiguration.BusId, accelerometerConfiguration.Line)
    {
      ClockFrequency = Adxl345.SpiClockFrequency,
      Mode = Adxl345.SpiMode,
    };

    _device = SpiDevice.Create(settings);
    _sensor = new Adxl345(_device, GravityRange.Range02);

    _ = CalibrateSensorAsync();
  }

  public void Dispose()
  {
    _device.Dispose();
  }

  public async Task<CurrentGForces> ReadAsync(CancellationToken cancelToken)
  {
    if (_baselineAcc is null)
    {
      return new CurrentGForces(NetForce: 0)
      {
        TestDefinition = _globalState.ActiveDefinition,
        TestStep = _globalState.ActiveStep,
      };
    }

    Vector3 data = _sensor.Acceleration - _baselineAcc.Value;

    double magnitude = Math.Sqrt(data.X * data.X + data.Y * data.Y + data.Z * data.Z);

    CurrentGForces result = new((float)magnitude)
    {
      TestDefinition = _globalState.ActiveDefinition,
      TestStep = _globalState.ActiveStep,
    };

    return result;
  }

  private async Task CalibrateSensorAsync(CancellationToken cancelToken = default)
  {
    TimeSpan duration = TimeSpan.FromSeconds(seconds: 5);
    TimeSpan interval = TimeSpan.FromMilliseconds(1);

    await Task.Delay(TimeSpan.FromSeconds(seconds: 5), cancelToken);

    List<Vector3> measurements = new();
    
    for (int i = 0; i < duration.TotalMilliseconds / interval.TotalMilliseconds; i++)
    {
      measurements.Add(_sensor.Acceleration);
      await Task.Delay(interval, cancelToken);
    }

    int count = measurements.Count;
    int trimCount = (int)(count * 0.05);

    float minXBefore = measurements.Min(v => v.X);
    float minYBefore = measurements.Min(v => v.Y);
    float minZBefore = measurements.Min(v => v.Z);

    var sortedX = measurements.Select(v => v.X).OrderBy(x => x).Skip(trimCount).Take(count - 2 * trimCount).ToList();
    var sortedY = measurements.Select(v => v.Y).OrderBy(y => y).Skip(trimCount).Take(count - 2 * trimCount).ToList();
    var sortedZ = measurements.Select(v => v.Z).OrderBy(z => z).Skip(trimCount).Take(count - 2 * trimCount).ToList();

    float minXAfter = sortedX.Min();
    float minYAfter = sortedY.Min();
    float minZAfter = sortedZ.Min();

    _logger.LogInformation(
      "Min values before/after trim (X/Y/Z): [{beforeX}/{afterX} {beforeY}/{afterY} {beforeZ}/{afterZ}]",
      minXBefore, minXAfter,
      minYBefore, minYAfter,
      minZBefore, minZAfter
    );

    _baselineAcc = new Vector3(
      sortedX.Average(),
      sortedY.Average(),
      sortedZ.Average()
    );

    _logger.LogInformation(
      "Determined baseline vector for acceleration: [{X} {Y} {Z}] over {cnt} values (trimmed {trim} top/bottom 5%)",
      _baselineAcc.Value.X,
      _baselineAcc.Value.Y,
      _baselineAcc.Value.Z,
      measurements.Count,
      trimCount
    );
  }
}