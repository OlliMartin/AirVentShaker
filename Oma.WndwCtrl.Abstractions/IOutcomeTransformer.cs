using LanguageExt;
using Oma.WndwCtrl.Abstractions.Errors;
using Oma.WndwCtrl.Abstractions.Model;

namespace Oma.WndwCtrl.Abstractions;

public interface IOutcomeTransformer
{
    Task<Either<FlowError, TransformationOutcome>> TransformCommandOutcomeAsync(
        Either<FlowError, CommandOutcome> commandOutcome, CancellationToken cancelToken = default
    );
}