using System.Text.Json.Serialization;

namespace Oma.WndwCtrl.Configuration.Model;

public class ComponentConfiguration
{
    [JsonPropertyName("__meta")]
    [JsonPropertyOrder(int.MaxValue)]
    public ConfigurationMetadata Metadata { get; set; } = new();
}