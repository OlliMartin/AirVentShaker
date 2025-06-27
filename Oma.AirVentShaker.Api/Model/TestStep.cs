namespace Oma.AirVentShaker.Api.Model;

public class TestStep
{
  public bool Active { get; set; } = true;

  public int Order { get; set; } = int.MaxValue;
  
  public float Frequency { get; set; }

  public TimeSpan Duration { get; set; }

  public int DurationInSeconds
  {
    get => (int)Duration.TotalSeconds;
    set => Duration = TimeSpan.FromSeconds(value);
  }

  public float TargetGravitationalForce { get; set; }

  public float Amplitude { get; set; } = 0.05f;

  public override string ToString() =>
    $"[{Order}] Freq={Frequency}Hz;Dur={Duration};Target={TargetGravitationalForce}G;CurrA={Amplitude}";
}