using Oma.WndwCtrl.Abstractions;
using Scalar.AspNetCore;

namespace Oma.WndwCtrl.Api;

public class CtrlApiService : IApiService
{
    private readonly ILogger _logger;
    private WebApplication? _app;

    public CtrlApiService(ILogger<CtrlApiService> logger)
    {
        _logger = logger;
    }
    
    public Task RunAsync(CancellationToken cancelToken = default)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();

        IMvcCoreBuilder mvcBuilder = builder.Services
            .AddMvcCore()
            .AddApiExplorer();
        
        builder.Services.AddOpenApi();
        
        _app = builder.Build();
        
        _app.MapControllers();
        
        _app.MapOpenApi();
        _app.MapScalarApiReference();
        
#if DEBUG
        _app.UseDeveloperExceptionPage();
#endif
        
        return _app.RunAsync(cancelToken);
    }

    public Task ForceStopAsync(CancellationToken cancelToken)
    {
        if (_app is null)
        {
            _logger.LogWarning("Force stop called without app running.");
            return Task.CompletedTask;
        }

        return _app.StopAsync(cancelToken);
    }
}