using Oma.WndwCtrl.Abstractions;

namespace Oma.WndwCtrl.MgmtApi.Model;

public interface IServiceWrapper
{    
    string Name { get; }
    
    Guid ServiceGuid { get; }
    
    DateTime? StartedAt { get; }
    
    ServiceStatus Status { get; }
}

public interface IServiceWrapper<out TService> : IService, IServiceWrapper
    where TService : class, IService
{
    string IServiceWrapper.Name => typeof(TService).Name;
}

public sealed record ServiceWrapper<TService> : IDisposable, IServiceWrapper<TService>
    where TService : class, IService
{
    public ServiceWrapper(TService service)
    {
        Service = service;
        ServiceGuid = Guid.NewGuid();
    }

    private TService Service { get; }

    public Guid ServiceGuid { get; }
    public DateTime? StartedAt { get; private set; }
    public ServiceStatus Status { get; private set; }

    private CancellationTokenSource CancellationTokenSource { get; set; }
    
    private Task? ServiceTask { get; set; }
    
    public void Dispose()
    {
        CancellationTokenSource.Dispose();
    }

    public Task RunAsync(CancellationToken cancelToken = default)
    {
        CancellationTokenSource = new();
        using CancellationTokenSource linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancelToken, CancellationTokenSource.Token);
        
        Status = ServiceStatus.Starting;
        
        ServiceTask = Service.RunAsync(linkedTokenSource.Token);
        
        StartedAt = DateTime.UtcNow;
        Status = ServiceStatus.Running;
        
        return ServiceTask;
    }

    public Task ForceStopAsync(CancellationToken cancelToken = default) => Service.ForceStopAsync(cancelToken);
}