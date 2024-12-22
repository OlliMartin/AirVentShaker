using Oma.WndwCtrl.Abstractions;
using Oma.WndwCtrl.Configuration.Model;
using Oma.WndwCtrl.CoreAsp;
using Oma.WndwCtrl.CoreAsp.Conventions;
using Scalar.AspNetCore;

namespace Oma.WndwCtrl.Api;

public class CtrlApiService : WebApplicationWrapper<CtrlApiService>, IApiService
{
    private readonly ILogger _logger;
    private readonly ComponentConfigurationAccessor _configurationAccessor;
    
    public CtrlApiService(ILogger<CtrlApiService> logger, ComponentConfigurationAccessor configurationAccessor)
    {
        _logger = logger;
        _configurationAccessor = configurationAccessor;
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services) => base
        .ConfigureServices(services)
        .AddSingleton(_configurationAccessor);
    
    public Task ForceStopAsync(CancellationToken cancelToken)
    {
        if (Application is null)
        {
            _logger.LogWarning("Force stop called without app running.");
            return Task.CompletedTask;
        }

        return Application.StopAsync(cancelToken);
    }
}