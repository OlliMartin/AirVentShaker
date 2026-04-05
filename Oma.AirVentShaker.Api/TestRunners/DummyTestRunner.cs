using Oma.AirVentShaker.Api.Interfaces;
using Oma.AirVentShaker.Api.Model;

namespace Oma.AirVentShaker.Api.TestRunners;

public class DummyTestRunner(
  ILogger<DummyTestRunner> logger,
  GlobalState globalState, 
  IAudioService audioService) : ITestRunner
{
  public async Task<TestSummary> ExecuteAsync(TestDefinition testDefinition, CancellationToken cancelToken)
  {
    Guid testId = Guid.NewGuid();
    globalState.ActiveDefinition = testDefinition;
    globalState.Stage = TestStage.Calibrate;

    logger.LogInformation("Starting test {TestName} with id {TestId}", testDefinition.Name, testId);
    
    foreach (TestStep testStep in testDefinition.Steps.Where(s => s.Active))
    {
      logger.LogInformation(
        "Processing step {StepOrder} | Target GForce: {GForce} with frequency {Frequency}Hz and duration {Duration}ms", 
        testStep.Order, 
        testStep.TargetGravitationalForce, 
        testStep.Frequency, 
        testStep.Duration.TotalMilliseconds);
      
      logger.LogDebug("Current amplitude is {Amplitude}", testStep.Amplitude);
      
      globalState.ActiveStep = testStep;

      await audioService.PlayAsync(
        new SineWaveDescriptor()
        {
          Frequency = testStep.Frequency,
          Amplitude = testStep.Amplitude == 0f ? 0.5f : testStep.Amplitude,
        },
        testStep.Duration + TimeSpan.FromMilliseconds(milliseconds: 500),
        cancelToken
      );

      await Task.Delay(testStep.Duration, cancelToken);
    }

    // globalState.Reset();

    return new TestSummary()
    {
      Id = testId,
      Status = "Finished",
    };
  }
}