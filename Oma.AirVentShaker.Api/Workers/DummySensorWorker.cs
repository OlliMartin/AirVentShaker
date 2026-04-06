using Oma.AirVentShaker.Api.Interfaces;
using Oma.AirVentShaker.Api.Model;
using Oma.AirVentShaker.Api.Model.Events;
using Oma.WndwCtrl.Abstractions.Messaging.Interfaces;

namespace Oma.AirVentShaker.Api.Workers;

public class DummySensorWorker(
  GlobalState globalState,
  IMessageBusWriter messageBusWriter) : IHostedService
{
  private readonly CancellationTokenSource _cts = new();
  private bool _isRunning;
  
  public async Task StartAsync(CancellationToken cancellationToken)
  {
    _isRunning = true;

    _ = ProcessAsync();
  }

  public async Task StopAsync(CancellationToken cancellationToken)
  {
    _isRunning = false;

    await _cts.CancelAsync();
    _cts.Dispose();
  }
  
  private async Task ProcessAsync()
  {
    while (_isRunning)
    {
      await Task.Delay(1000);
      
      GForceValueBatchEvent @event = new()
      {
        DataPoints = Enumerable.Range(0, 2000)
          .Select(_ => Random.Shared.NextSingle())
          .Select(val => new CurrentGForces(val)
          {
            TestDefinition = globalState.ActiveDefinition,
            TestStep = globalState.ActiveStep,
          })
          .ToList(),
      };

      await messageBusWriter.SendAsync(@event, _cts.Token);
    }
  }
}