using Oma.AirVentShaker.Api.Model;

namespace Oma.AirVentShaker.Api.Interfaces;

public interface ITestRunner
{
  ILogger Logger { get; }

  GlobalState GlobalState { get; }
  
  Task<TestSummary> CalibrateAsync(TestDefinition testDefinition, CancellationToken cancelToken)
  {
    GlobalState.ActiveDefinition = testDefinition;
    GlobalState.Stage = TestStage.Calibrate;
    return RunInternalAsync(testDefinition, cancelToken); 
  }
  
  Task<TestSummary> RunAsync(TestDefinition testDefinition, CancellationToken cancelToken)
  {
    GlobalState.ActiveDefinition = testDefinition;
    GlobalState.Stage = TestStage.Run;
    return RunInternalAsync(testDefinition, cancelToken); 
  }
  
  async Task<TestSummary> RunInternalAsync(TestDefinition testDefinition, CancellationToken cancelToken)
  {
    Guid testId = Guid.NewGuid();
    
    Logger.LogInformation("Starting test {TestName} with id {TestId}", testDefinition.Name, testId);

    foreach (TestStep testStep in testDefinition.Steps.Where(s => s.Active))
    {
      Logger.LogInformation(
        "Processing step {StepOrder} | Target GForce: {GForce} with frequency {Frequency}Hz and duration {Duration}ms", 
        testStep.Order, 
        testStep.TargetGravitationalForce, 
        testStep.Frequency, 
        testStep.Duration.TotalMilliseconds);
      
      Logger.LogDebug("Current amplitude is {Amplitude}", testStep.Amplitude);
      
      GlobalState.ActiveStep = testStep;

      _ = await ExecuteStepAsync(testDefinition, testStep, cancelToken);
      
      testStep.IsCalibrated = true;
    }
    
    GlobalState.Stage = TestStage.Calibrated;
    
    return new TestSummary()
    {
      Id = testId,
      Status = "Finished",
    };
  }

  Task<TestStep> ExecuteStepAsync(TestDefinition testDefinition, TestStep testStep, CancellationToken cancelToken);
}