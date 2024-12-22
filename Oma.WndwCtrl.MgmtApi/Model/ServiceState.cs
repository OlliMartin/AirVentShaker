using Oma.WndwCtrl.Abstractions;

namespace Oma.WndwCtrl.MgmtApi.Model;

public class ServiceState
{
    public IReadOnlyList<IServiceWrapper<IApiService>> ApiServices { get; init; }
    
    public ServiceState(
        IEnumerable<IApiService> apiServices
    )
    {
        ApiServices = apiServices.Select(svc => new ServiceWrapper<IApiService>(svc)).ToList().AsReadOnly();
    }
    
    public IEnumerable<IServiceWrapper<IService>> All => ApiServices;
}