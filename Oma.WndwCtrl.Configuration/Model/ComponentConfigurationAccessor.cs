using System.Text.Json;

namespace Oma.WndwCtrl.Configuration.Model;

public class ComponentConfigurationAccessor
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    public ComponentConfiguration Configuration { get; set; } = new();
    
    public async static Task<ComponentConfigurationAccessor> FromFileAsync(CancellationToken cancelToken = default)
    {
        const string configurationFilePath = "component-configuration.json";

        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configurationFilePath);
        string fileContent = await File.ReadAllTextAsync(filePath, cancelToken);
        ComponentConfiguration? configuration = JsonSerializer.Deserialize<ComponentConfiguration>(fileContent, _jsonOptions);

        if (configuration is null)
        {
            throw new InvalidOperationException("Could not load component configuration.");
        }

        return new()
        {
            Configuration = configuration,
        };
    }
}