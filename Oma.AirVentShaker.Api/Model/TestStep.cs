namespace Oma.AirVentShaker.Api.Model;

public class TestStep
{
  public bool Active { get; set; } = true;

  public int Order { get; set; } = int.MaxValue;
  
  public float Frequency { get; set; }

  public TimeSpan Duration { get; set; }

  public TestDefinition? TestDefinition { get; set; }

  public TestStep WithTestDefinition(TestDefinition testDefinition)
  {
    TestDefinition = testDefinition;
    return this;
  }
  
  public int DurationInSeconds
  {
    get => (int)Duration.TotalSeconds;
    set => Duration = TimeSpan.FromSeconds(value);
  }

  public float TargetGravitationalForce { get; set; }

  public float Amplitude { get; set; } = 0.05f;
  
  public bool IsCalibrated { get; set; }

  public string AmplitudeUi
  {
    get => IsCalibrated
      ? Amplitude.ToString("d2")
      : "?";
  }

  public override string ToString() =>
    $"[{Order}] Freq={Frequency}Hz;Dur={Duration};Target={TargetGravitationalForce}G;CurrA={Amplitude}";
}