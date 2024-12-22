namespace Oma.WndwCtrl.Abstractions;

public interface IService
{
    Task StartAsync(CancellationToken cancelToken = default);
    
    Task ForceStopAsync(CancellationToken cancelToken = default);

    Task WaitForShutdownAsync(CancellationToken cancelToken = default);
}