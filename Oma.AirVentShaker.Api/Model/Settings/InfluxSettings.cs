namespace Oma.AirVentShaker.Api.Model.Settings;

public class InfluxSettings
{
  public const string SectionName = "Influx";

  public string Url { get; init; } = string.Empty;
  public string Token { get; init; } = string.Empty;
  
  public string Org { get; init; } = string.Empty;
  public string Bucket { get; init; } = string.Empty;
}