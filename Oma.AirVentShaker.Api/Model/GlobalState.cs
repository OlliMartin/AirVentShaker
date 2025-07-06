namespace Oma.AirVentShaker.Api.Model;

public enum TestStage
{
  Idle,
  Calibrate,
  Calibrated,
  Run,
}

public class GlobalState
{
  public bool CanRunTest => Stage == TestStage.Calibrated;

  public bool CannotRunTest => CanRunTest is false;
  
  public GlobalState()
  {
    ActiveDefinition = new(this);
    ActiveDefinition.AfterInit();
  }
  
  public event EventHandler OnChange;
  
  public TestStage Stage { get; set; } = TestStage.Idle;

  public string StageUi => Stage.ToString();

  public TestStep? ActiveStep { get; set; }

  public TestDefinition ActiveDefinition { get; set; }

  public GlobalState AddTestStep(TestStep testStep)
  {
    UpdateTestSteps(
      ActiveSteps.Concat([testStep.WithTestDefinition(ActiveDefinition)]));

    ActiveDefinition.RaiseChange();
    OnChange?.Invoke(this, EventArgs.Empty);
    
    return this;
  }

  public IList<TestStep> ActiveSteps => ActiveDefinition.Steps;
  
  public GlobalState RemoveTestStep(TestStep testStep)
  {
    UpdateTestSteps(
      ActiveSteps.Where(ts => ts.Equals(testStep) is false));

    ActiveDefinition.RaiseChange();
    OnChange?.Invoke(this, EventArgs.Empty);
    
    return this;
  }
  
  public void UpdateTestSteps(IEnumerable<TestStep> newSteps)
  {
    var newStepsInt = newSteps.ToList();
    
    ActiveSteps.Clear();
    ActiveDefinition.Steps.AddRange(newStepsInt);
  
    ActiveDefinition.RaiseChange();
    OnChange?.Invoke(newSteps, EventArgs.Empty);
  }

  public void Reset()
  {
    Stage = TestStage.Idle;
    
    ActiveDefinition = new(this);
    ActiveDefinition.AfterInit();
    
    ActiveStep = null;
  }
}