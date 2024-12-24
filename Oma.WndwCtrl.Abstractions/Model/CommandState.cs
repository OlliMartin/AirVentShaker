using LanguageExt;
using Microsoft.Extensions.Logging;

namespace Oma.WndwCtrl.Abstractions.Model;

public record CommandState
{
    public ILogger Logger;
    public IEnumerable<ICommandExecutor> CommandExecutors;
    
    public ICommand Command { get; }
    
    public Option<DateTime> StartDate { get; init; }

    public TimeSpan? ExecutionDuration { get; set; }
    public int? ExecutedRetries { get; set; }
    
    public CommandState(ILogger logger, IEnumerable<ICommandExecutor> commandExecutors, ICommand command)
    {
        Logger = logger;
        CommandExecutors = commandExecutors;
        Command = command;
    }
}