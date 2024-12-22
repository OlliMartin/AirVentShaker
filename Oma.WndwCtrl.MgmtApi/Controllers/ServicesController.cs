using Microsoft.AspNetCore.Mvc;
using Oma.WndwCtrl.MgmtApi.Model;

namespace Oma.WndwCtrl.MgmtApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ServicesController : ControllerBase
{
    private readonly ServiceState _serviceState;

    public ServicesController(ServiceState serviceState)
    {
        _serviceState = serviceState;
    }

    [HttpGet]
    [EndpointName("Test")]
    [EndpointSummary("ABC")]
    [EndpointDescription("ABC")]
    [Produces("application/json")]
    public IActionResult GetAll()
        => Ok(_serviceState.All.Cast<IServiceWrapper>());
}