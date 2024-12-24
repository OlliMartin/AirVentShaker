using LanguageExt;
using LanguageExt.Common;

namespace Oma.WndwCtrl.Abstractions.Errors;

public record CommandError : FlowError, ICommandExecutionMetadata
{
    private CommandError(TechnicalError technicalError) : base(technicalError)
    {
    }
    
    public override Option<Error> Inner { get; } = Option<Error>.None;

    public Option<TimeSpan> ExecutionDuration { get; set; } = Option<TimeSpan>.None;
    public Option<int> ExecutedRetries { get; set; } = Option<int>.None;

    public static implicit operator CommandError(TechnicalError error)
        => new(error);
}