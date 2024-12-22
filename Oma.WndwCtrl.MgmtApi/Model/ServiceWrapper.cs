using System.Diagnostics.CodeAnalysis;
using Oma.WndwCtrl.Abstractions;
using Oma.WndwCtrl.Abstractions.Model;

namespace Oma.WndwCtrl.MgmtApi.Model;


public sealed record ServiceWrapper<TService> : IDisposable, IServiceWrapper<TService>
    where TService : class, IService
{
    public ServiceWrapper(TService service)
    {
        Service = service;
        ServiceGuid = Guid.NewGuid();
    }

    public TService Service { get; }

    public Guid ServiceGuid { get; }
    public DateTime? StartedAt { get; private set; }
    public ServiceStatus Status { get; private set; }

    private CancellationTokenSource? CancellationTokenSource { get; set; }
    
    private Task? ServiceTask { get; set; }
    
    public Task RunAsync(CancellationToken cancelToken = default)
    {
        CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancelToken);
        
        Status = ServiceStatus.Starting;
        
        ServiceTask = Service.RunAsync(CancellationTokenSource.Token);
        
        StartedAt = DateTime.UtcNow;
        Status = ServiceStatus.Running;
        
        return ServiceTask;
    }
    
    public async Task StopAsync(CancellationToken cancelToken = default)
    {
        if (!IsRunning())
        {
            return;
        }
        
        TimeSpan stopTimeout = TimeSpan.FromSeconds(5);
        
        CancellationTokenSource ctsForceCancelAfter = CancellationTokenSource.CreateLinkedTokenSource(cancelToken);
        ctsForceCancelAfter.CancelAfter(stopTimeout);

        try
        {
            Status = ServiceStatus.Stopping;
            await CancellationTokenSource.CancelAsync();

            await Task.WhenAny(
                ServiceTask ?? Task.CompletedTask,
                Task.Delay(stopTimeout, ctsForceCancelAfter.Token)
            );
            
            ctsForceCancelAfter.Token.ThrowIfCancellationRequested();
        }
        catch (OperationCanceledException)
        {
            await ForceStopAsync(cancelToken);
        }
        finally
        {
            CancellationTokenSource.Dispose();
            Status = ServiceStatus.Stopped;

            CancellationTokenSource = null;
        }
    }

    public Task ForceStopAsync(CancellationToken cancelToken = default) => Service.ForceStopAsync(cancelToken);

    [MemberNotNullWhen(true, nameof(CancellationTokenSource))]
    private bool IsRunning()
        => Status == ServiceStatus.Running && CancellationTokenSource is not null;
    
    public void Dispose()
    {
        CancellationTokenSource?.Dispose();
        CancellationTokenSource = null;
    }
}