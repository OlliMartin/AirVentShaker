using Oma.WndwCtrl.Abstractions;
using Scalar.AspNetCore;

namespace Oma.WndwCtrl.Api;

public class CtrlApiService : IApiService
{
    public Task RunAsync(CancellationToken cancelToken = default)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();

        builder.Services.AddOpenApi();
        
        WebApplication app = builder.Build();
        
        app.MapOpenApi();
        app.MapScalarApiReference();
        
        return app.RunAsync(cancelToken);
    }

    public Task ForceStopAsync(CancellationToken cancelToken)
    {
        throw new NotImplementedException();
    }
}