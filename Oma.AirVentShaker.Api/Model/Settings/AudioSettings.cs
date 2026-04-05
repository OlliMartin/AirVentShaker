namespace Oma.AirVentShaker.Api.Model.Settings;

public class AudioSettings
{
  public const string SectionName = "Audio";
  
  public string PlaybackDevice { get; init; } = string.Empty;
}