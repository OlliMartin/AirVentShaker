using Oma.AirVentShaker.Api.Interfaces;
using Oma.AirVentShaker.Api.Model;

namespace Oma.AirVentShaker.Api.TestRunners;

public class CalibratingTestRunner(
  ILogger<CalibratingTestRunner> logger,
  GlobalState globalState, 
  IAudioService audioService) : ITestRunner
{
  public ILogger Logger => logger;
  public GlobalState GlobalState => globalState;
  
  public async Task<TestStep> ExecuteStepAsync(TestDefinition testDefinition, TestStep testStep, CancellationToken cancelToken)
  {
    bool isCalibrating = globalState.Stage == TestStage.Calibrate;

    TimeSpan duration = isCalibrating
      ? globalState.CalibrationDuration
      : testStep.Duration;
    
    await audioService.PlayAsync(
      new SineWaveDescriptor()
      {
        Frequency = testStep.Frequency,
        Amplitude = testStep.Amplitude,
      },
      duration + TimeSpan.FromMilliseconds(milliseconds: 500),
      cancelToken
    );

    await Task.Delay(duration, cancelToken);
    
    testStep.Amplitude = audioService.LastAmplitude;

    return testStep;
  }
}