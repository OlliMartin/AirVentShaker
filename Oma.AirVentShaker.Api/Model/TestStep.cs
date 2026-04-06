namespace Oma.AirVentShaker.Api.Model;

public class TestStep
{
  private float _amplitude = 0.05f;
  private float? _measuredGravitationalForce;
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

  public float? MeasuredGravitationalForce
  {
    get => _measuredGravitationalForce;
    set
    {
      _measuredGravitationalForce = value;
      TestDefinition?.RaiseChange();
    }
  }
  
  public string MesauredGForceUi
  {
    get => MeasuredGravitationalForce is not null
      ? MeasuredGravitationalForce.Value.ToString("F4")
      : "?";
  }

  public float Amplitude
  {
    get => _amplitude;
    set
    {
      TestDefinition?.RaiseChange();
      _amplitude = value;
    }
  }

  public bool IsCalibrated { get; set; }

  public string AmplitudeUi
  {
    get => Amplitude.ToString("F5") + (IsCalibrated ? "" : " ??");
  }

  public override string ToString() =>
    $"[{Order}] Freq={Frequency}Hz;Dur={Duration};Target={TargetGravitationalForce}G;CurrA={Amplitude}";
}