using Oma.AirVentShaker.Api.Model;
using Oma.AirVentShaker.Api.Model.Events;
using Oma.WndwCtrl.Abstractions.Messaging.Interfaces;

namespace Oma.AirVentShaker.Api.Messaging.Consumers;

public class PersistMeasuredGForceMessageConsumer(
  ILogger<PersistMeasuredGForceMessageConsumer> logger,
  GlobalState globalState) : IMessageConsumer<GForceAggregatedMeasurementEvent>
{
  public bool IsSubscribedTo(IMessage message) => message is GForceAggregatedMeasurementEvent;

  public Task OnExceptionAsync(IMessage message, Exception exception, CancellationToken cancelToken = default)
  {
    logger.LogError(
      exception,
      "An unexpected error occurred processing event {type}.",
      message.GetType().Name
    );

    return Task.CompletedTask;
  }

  public Task OnMessageAsync(
    GForceAggregatedMeasurementEvent message,
    CancellationToken cancelToken = default
  )
  {
    if (globalState.ActiveStep is null)
    {
      return Task.CompletedTask;  
    }
    
    globalState.ActiveStep.MeasuredGravitationalForce = message.Data;
    return Task.CompletedTask;
  }
}