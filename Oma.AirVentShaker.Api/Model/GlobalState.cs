namespace Oma.AirVentShaker.Api.Model;

public enum TestStage
{
  Idle,
  Calibrate,
  Run,
}

public class GlobalState
{
  public TestStage Stage { get; set; } = TestStage.Idle;

  public TestStep? ActiveStep { get; set; }

  public TestDefinition ActiveDefinition { get; set; } = new();

  public void Reset()
  {
    Stage = TestStage.Idle;
    ActiveDefinition = new();
    ActiveStep = null;
  }
}