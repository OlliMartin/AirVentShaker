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
  private TestStage _stage = TestStage.Idle;
  public bool CanRunTest => Stage == TestStage.Calibrated;

  public bool CannotRunTest => CanRunTest is false;
  
  public GlobalState()
  {
    ActiveDefinition = new(this);
    ActiveDefinition.AfterInit();
  }
  
  public event EventHandler OnChange;

  public TestStage Stage
  {
    get => _stage;
    set
    {
      _stage = value;
      OnChange?.Invoke(this, EventArgs.Empty);
    }
  }

  public string StageUi => Stage.ToString();

  public TestStep? ActiveStep { get; set; }

  public TestDefinition ActiveDefinition { get; set; }

  public TimeSpan CalibrationDuration { get; set; } = TimeSpan.FromSeconds(15);
  
  public int CalibrationDurationInSeconds
  {
    get => (int)CalibrationDuration.TotalSeconds;
    set => CalibrationDuration = TimeSpan.FromSeconds(value);
  }

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
    List<TestStep> nextSteps = ActiveSteps
      .Where(ts => ts.Equals(testStep) is false)
      .ToList();

    for (int i = 0; i < nextSteps.Count; i++)
    {
      nextSteps[i].Order = i + 1;
    }
    
    UpdateTestSteps(nextSteps);

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