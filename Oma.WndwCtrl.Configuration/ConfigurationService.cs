using System.Text.Json;
using Oma.WndwCtrl.Abstractions;
using Oma.WndwCtrl.Configuration.Model;

namespace Oma.WndwCtrl.Configuration;

public class ConfigurationService(ComponentConfigurationAccessor componentConfigurationAccessor)
    : IBackgroundService
{
    public async Task StartAsync(CancellationToken cancelToken = default)
    {
        const string configurationFilePath = "component-configuration.json";

        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configurationFilePath);
        string fileContent = await File.ReadAllTextAsync(filePath, cancelToken);
        componentConfigurationAccessor.Configuration = JsonSerializer.Deserialize<ComponentConfiguration>(fileContent);
    }

    public Task ForceStopAsync(CancellationToken cancelToken = default)
        => Task.CompletedTask;

    public Task WaitForShutdownAsync(CancellationToken cancelToken = default)
        => Task.CompletedTask;
}
