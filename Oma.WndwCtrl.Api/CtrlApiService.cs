using Oma.WndwCtrl.Abstractions;
using Oma.WndwCtrl.CoreAsp;
using Oma.WndwCtrl.CoreAsp.Conventions;
using Scalar.AspNetCore;

namespace Oma.WndwCtrl.Api;

public class CtrlApiService : WebApplicationWrapper<CtrlApiService>, IApiService
{
    private readonly ILogger _logger;

    public CtrlApiService(ILogger<CtrlApiService> logger)
    {
        _logger = logger;
    }
    
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