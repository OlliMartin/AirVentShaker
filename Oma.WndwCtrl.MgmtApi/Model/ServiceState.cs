using Oma.WndwCtrl.Abstractions;

namespace Oma.WndwCtrl.MgmtApi.Model;

public class ServiceState
{
    public IReadOnlyList<IServiceWrapper<IBackgroundService>> BackgroundServices { get; init; }
    public IReadOnlyList<IServiceWrapper<IApiService>> ApiServices { get; init; }
    
    public ServiceState(
        IEnumerable<IApiService> apiServices,
        IEnumerable<IBackgroundService> backgroundServices
    )
    {
        BackgroundServices = backgroundServices.Select(svc => new ServiceWrapper<IBackgroundService>(svc)).ToList().AsReadOnly();
        ApiServices = apiServices.Select(svc => new ServiceWrapper<IApiService>(svc)).ToList().AsReadOnly();
    }
    
    public IEnumerable<IServiceWrapper<IService>> All => BackgroundServices
        .Concat<IServiceWrapper<IService>>(ApiServices);
}