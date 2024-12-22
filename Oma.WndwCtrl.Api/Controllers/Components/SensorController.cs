using Microsoft.AspNetCore.Mvc;
using Oma.WndwCtrl.Core.Model;

namespace Oma.WndwCtrl.Api.Controllers.Components;

[ApiController]
[Route("components/{componentName}")]
public class SensorController : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Query Sensor")]
    public async Task<IActionResult> QueryAsync()
    {
        return this.Ok("something hits the controller.");
    }
}