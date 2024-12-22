using Microsoft.AspNetCore.Mvc;
using Oma.WndwCtrl.Abstractions;
using Oma.WndwCtrl.Abstractions.Model;
using Oma.WndwCtrl.MgmtApi.Model;
using ServiceDescriptor = Oma.WndwCtrl.MgmtApi.Model.Api.ServiceDescriptor;

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
    [EndpointName($"Services_{nameof(GetAll)}")]
    [EndpointSummary("Service List")]
    [EndpointDescription("Lists all registered services")]
    [Produces("application/json")]
    public IActionResult GetAll()
        => Ok(_serviceState.All.Select(ServiceDescriptor.FromServiceWrapper));

    [HttpGet("{serviceGuid:guid}")]
    [EndpointName($"Services_{nameof(GetByGuid)}")]
    [EndpointSummary("Service Details by GUID")]
    [EndpointDescription("Retrieves service details by GUID")]
    [Produces("application/json")]
    public IActionResult GetByGuid([FromRoute] Guid serviceGuid) => GetDetail(sw => sw.ServiceGuid == serviceGuid);
    
    [HttpGet("{serviceName}")]
    [EndpointName($"Services_{nameof(GetByName)}")]
    [EndpointSummary("Service Details by Name")]
    [EndpointDescription("Retrieves service details by Name")]
    [Produces("application/json")]
    public IActionResult GetByName([FromRoute] string serviceName) => GetDetail(sw => sw.Name == serviceName);
    
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult GetDetail(Func<IServiceWrapper, bool> predicate)
    {
        IServiceWrapper? service = _serviceState.All.FirstOrDefault(predicate);
        
        return service is null 
            ? Problem("Could not locate service", statusCode: 404) 
            : Ok(ServiceDescriptor.FromServiceWrapper(service));
    }
    
        
    [HttpPost("{serviceName}/start")]
    public Task<IActionResult> StartByNameAsync([FromRoute] string serviceName) => StartAsync(sw => sw.Name == serviceName);

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> StartAsync(Func<IServiceWrapper, bool> predicate)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        IServiceWrapper? service = _serviceState.All.FirstOrDefault(predicate);

        if (service is null)
        {
            return Problem("Could not locate service to start", statusCode: 404);
        }
        
        _ = service.RunAsync(cancelToken: CancellationToken.None);

        return Ok();
    }
    
    [HttpPost("{serviceName}/stop")]
    public Task<IActionResult> StopByNameAsync([FromRoute] string serviceName) => StopAsync(sw => sw.Name == serviceName);

    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> StopAsync(Func<IServiceWrapper, bool> predicate)
    {
        IServiceWrapper? service = _serviceState.All.FirstOrDefault(predicate);

        if (service is null)
        {
            return Problem("Could not locate service to stop", statusCode: 404);
        }

        if (service.Status is not ServiceStatus.Running)
        {
            return Problem($"Service {service.Name} is not running", statusCode: 400);
        }

        await service.StopAsync(cancelToken: HttpContext.RequestAborted);

        return Ok();
    }
}