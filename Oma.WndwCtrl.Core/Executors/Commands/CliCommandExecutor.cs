using LanguageExt;
using static LanguageExt.Prelude;
using Oma.WndwCtrl.Abstractions;
using Oma.WndwCtrl.Abstractions.Errors;
using Oma.WndwCtrl.Abstractions.Model;
using Oma.WndwCtrl.Core.Model.Commands;

namespace Oma.WndwCtrl.Core.Executors.Commands;

public class CliCommandExecutor : ICommandExecutor<CliCommand>
{
    public async Task<Either<FlowError, CommandOutcome>> ExecuteAsync(
        CliCommand command, CancellationToken cancelToken = default
    )
    {
        return Right(new CommandOutcome()
        {
            OutcomeRaw = "This is a test cli outcome"
        });
    }
}