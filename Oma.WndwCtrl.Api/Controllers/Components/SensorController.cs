using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using Oma.WndwCtrl.Abstractions.Errors;
using Oma.WndwCtrl.Abstractions.Model;
using Oma.WndwCtrl.Core.FlowExecutors;
using Oma.WndwCtrl.Core.Model;

namespace Oma.WndwCtrl.Api.Controllers.Components;

public class SensorController : ComponentControllerBase<Sensor>
{
    [HttpGet]
    [EndpointSummary("Query Sensor")]
    public async Task<IActionResult> QueryAsync()
    {
        Either<FlowError, TransformationOutcome> flowResult = await FlowExecutor.ExecuteAsync(Component.QueryCommand, HttpContext.RequestAborted);

        return flowResult.BiFold<IActionResult>(
            state: null!,
            Right: (_, outcome) => Ok(outcome),
            Left: (_, error) => Problem(error.Message,  statusCode: error.IsExceptional ? 500 : 400)
        );
    }
}