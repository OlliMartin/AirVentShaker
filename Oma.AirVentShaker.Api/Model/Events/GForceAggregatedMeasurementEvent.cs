using Oma.WndwCtrl.Abstractions.Messaging.Model;

namespace Oma.AirVentShaker.Api.Model.Events;

public record GForceAggregatedMeasurementEvent(float Data) : Event
{
  public override string Type { get; } = "AirVentShaker";
  public override string Name { get; } = nameof(GForceAggregatedMeasurementEvent);
  public override string? ComponentName { get; } = null;
}