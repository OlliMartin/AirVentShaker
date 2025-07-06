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
    _globalState.AddTestStep(
        new()
        {
          Frequency = 10,
          Duration = TimeSpan.FromSeconds(5),
          TargetGravitationalForce = 0.3f,
          Order = 0,
        }
      )
      .AddTestStep(
        new()
        {
          Frequency = 20,
          Duration = TimeSpan.FromSeconds(5),
          TargetGravitationalForce = 0.3f,
          Order = 1,
        }
      )
      .AddTestStep(
        new()
        {
          Frequency = 40,
          Duration = TimeSpan.FromSeconds(5),
          TargetGravitationalForce = 0.3f,
          Order = 2,
        }
      )
      .AddTestStep(
        new()
        {
          Frequency = 50,
          Duration = TimeSpan.FromSeconds(5),
          TargetGravitationalForce = 0.3f,
          Order = 3,
        }
      );
  }

  public List<TestStep> Steps { get; set; } = new();
}