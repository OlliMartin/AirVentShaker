namespace Oma.AirVentShaker.Api.Model;

public class TestDefinition
{
  private readonly GlobalState _globalState;
  public string Name { get; init; } = $"Unnamed_Test_{DateTime.UtcNow:o}";

  public event EventHandler OnChange;

  internal void RaiseChange() => OnChange?.Invoke(this, EventArgs.Empty);

  public TestDefinition(GlobalState globalState)
  {
    _globalState = globalState;
  }

  public void AfterInit()
  {
    _globalState
      .AddTestStep(
        new()
        {
          Frequency = 40,
          Duration = TimeSpan.FromSeconds(15),
          TargetGravitationalForce = 0.3f,
          Order = 1,
        }
      )
      .AddTestStep(
        new()
        {
          Frequency = 50,
          Duration = TimeSpan.FromSeconds(15),
          TargetGravitationalForce = 0.3f,
          Order = 2,
        }
      );
  }

  public List<TestStep> Steps { get; set; } = new();
}