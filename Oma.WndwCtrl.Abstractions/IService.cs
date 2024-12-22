namespace Oma.WndwCtrl.Abstractions;

public interface IService
{
    Task RunAsync(CancellationToken cancelToken = default);
    
    Task ForceStopAsync(CancellationToken cancelToken = default);
}