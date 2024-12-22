using Oma.WndwCtrl.Abstractions;
using Oma.WndwCtrl.Api;
using Oma.WndwCtrl.CoreAsp.Conventions;
using Oma.WndwCtrl.MgmtApi.Model;
using Oma.WndwCtrl.MgmtApi.Workers;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("mgmt-api.config.json", optional: false, reloadOnChange: false);

IMvcCoreBuilder mvcBuilder = builder.Services
    .AddMvcCore(opts =>
    {
        opts.Conventions.Add(new ContainingAssemblyApplicationModelConvention<ServiceWorker>());
    })
    .AddApiExplorer();

builder.Services.AddOpenApi();

builder.Services
    .AddSingleton<ServiceState>()
    .AddSingleton<IApiService, CtrlApiService>();

builder.Services.AddHostedService<ServiceWorker>();

WebApplication app = builder.Build();

app.MapControllers();

app.MapOpenApi();
app.MapScalarApiReference();

#if DEBUG
    app.UseDeveloperExceptionPage();
#endif

app.Run();