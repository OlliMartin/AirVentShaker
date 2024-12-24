using Microsoft.AspNetCore.Mvc;
using Oma.WndwCtrl.Abstractions;
using Oma.WndwCtrl.Api.Conventions;
using Oma.WndwCtrl.Api.Extensions;
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

    protected override MvcOptions PreConfigureMvcOptions(MvcOptions options)
    {
        options.Conventions.Add(new ComponentApplicationConvention(_configurationAccessor));
        return base.PreConfigureMvcOptions(options);
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services) => base
        .ConfigureServices(services)
        .AddComponentApi()
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