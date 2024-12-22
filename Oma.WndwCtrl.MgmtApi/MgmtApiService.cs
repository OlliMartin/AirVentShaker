using Oma.WndwCtrl.Abstractions;
using Oma.WndwCtrl.Api;
using Oma.WndwCtrl.Api.Extensions;
using Oma.WndwCtrl.CoreAsp;
using Oma.WndwCtrl.MgmtApi.Model;
using Oma.WndwCtrl.MgmtApi.Workers;

namespace Oma.WndwCtrl.MgmtApi;

public class MgmtApiService : WebApplicationWrapper<MgmtApiService>
{
    protected override IConfigurationBuilder ConfigurationConfiguration(IConfigurationBuilder configurationBuilder)
    {
        return base.ConfigurationConfiguration(configurationBuilder)
            .AddJsonFile("mgmt-api.config.json", optional: false, reloadOnChange: false);
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services) =>
        base.ConfigureServices(services)
            .AddSingleton<ServiceState>()
            .AddComponentApi()
            .AddHostedService<ServiceWorker>();
}