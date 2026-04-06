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
          TargetGravitationalForce = 0.5f,
          Order = 2,
        }
      )
      .AddTestStep(
        new()
        {
          Frequency = 40,
          Duration = TimeSpan.FromSeconds(30),
          TargetGravitationalForce = 0.4f,
          Order = 3,
        }
      ).AddTestStep(
        new()
        {
          Frequency = 30,
          Duration = TimeSpan.FromSeconds(45),
          TargetGravitationalForce = 0.25f,
          Order = 4,
        }
      );
  }

  public List<TestStep> Steps { get; set; } = new();
}