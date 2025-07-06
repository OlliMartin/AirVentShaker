namespace Oma.AirVentShaker.Api.Model;

public class TestDefinition
{
  public string Name { get; init; } = $"Unnamed_Test_{DateTime.UtcNow:o}";

  public event EventHandler OnChange;

  public void UpdateTestSteps(IEnumerable<TestStep> newSteps)
  {
    var newStepsInt = newSteps.ToList();
    Steps.Clear();
    Steps.AddRange(newStepsInt);

    OnChange(newSteps, EventArgs.Empty);
  }
  
  public List<TestStep> Steps { get; set; } = new()
  {
    new TestStep()
    {
      Frequency = 10,
      Duration = TimeSpan.FromSeconds(5),
      TargetGravitationalForce = 0.3f,
      Order = 0,
    },
    new TestStep()
    {
      Frequency = 20,
      Duration = TimeSpan.FromSeconds(5),
      TargetGravitationalForce = 0.3f,
      Order = 1,
    },
    new TestStep()
    {
      Frequency = 40,
      Duration = TimeSpan.FromSeconds(5),
      TargetGravitationalForce = 0.3f,
      Order = 2,
    },
    new TestStep()
    {
      Frequency = 50,
      Duration = TimeSpan.FromSeconds(5),
      TargetGravitationalForce = 0.3f,
      Order = 3,
    },
  };
}