using Microsoft.AspNetCore.Mvc;
using Oma.WndwCtrl.Core.Model;

namespace Oma.WndwCtrl.Api.Controllers.Components;

public class SensorController : ComponentControllerBase<Sensor>
{
    [HttpGet]
    [EndpointSummary("Query Sensor")]
    public async Task<IActionResult> QueryAsync()
    {
        await Task.Delay(0);
        return Ok(Component.QueryCommand);
    }
}