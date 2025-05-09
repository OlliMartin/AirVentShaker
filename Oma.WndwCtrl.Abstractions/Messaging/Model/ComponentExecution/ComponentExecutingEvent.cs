using JetBrains.Annotations;

namespace Oma.WndwCtrl.Abstractions.Messaging.Model.ComponentExecution;

[PublicAPI]
public record ComponentExecutingEvent : ComponentEvent
{
  public ComponentExecutingEvent(
    ComponentToRunEvent componentToRun,
    int commandsToExecute,
    DateTime startedAt
  ) : base(
    componentToRun
  )
  {
    CommandsToExecute = commandsToExecute;
    StartedAt = startedAt;
    DelayedBy = componentToRun.DelayedBy;
  }

  public int CommandsToExecute { get; }
  public DateTime StartedAt { get; }

  public TimeSpan? DelayedBy { get; }
  public override string Type => "Scheduling";
  public override string Name => nameof(ComponentExecutingEvent);
}