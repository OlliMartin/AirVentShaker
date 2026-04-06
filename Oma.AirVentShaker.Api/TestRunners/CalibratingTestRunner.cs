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
    await audioService.PlayAsync(
      new SineWaveDescriptor()
      {
        Frequency = testStep.Frequency,
        Amplitude = testStep.Amplitude,
      },
      testStep.Duration + TimeSpan.FromMilliseconds(milliseconds: 500),
      cancelToken
    );

    await Task.Delay(testStep.Duration, cancelToken);
    testStep.Amplitude = audioService.LastAmplitude;

    return testStep;
  }
}