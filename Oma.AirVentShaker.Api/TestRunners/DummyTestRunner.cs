using Oma.AirVentShaker.Api.Interfaces;
using Oma.AirVentShaker.Api.Model;

namespace Oma.AirVentShaker.Api.TestRunners;

public class DummyTestRunner(
  ILogger<DummyTestRunner> logger,
  GlobalState globalState) : ITestRunner
{
  public ILogger Logger => logger;
  public GlobalState GlobalState => globalState;

  public async Task<TestStep> ExecuteStepAsync(
    TestDefinition testDefinition,
    TestStep testStep,
    CancellationToken cancelToken
  )
  {
    for (int i = 0; i < 3; i++)
    {
      await Task.Delay(TimeSpan.FromSeconds(1), cancelToken);
      testStep.Amplitude = Random.Shared.NextSingle();
    }

    return testStep;
  }
}