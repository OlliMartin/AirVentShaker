using Microsoft.AspNetCore.Mvc;
using Oma.WndwCtrl.Core.Model;

namespace Oma.WndwCtrl.Api.Controllers.Components;

[ApiController]
[Route("components/{componentName}")]
public class ComponentControllerBase<TComponent> : ControllerBase
    where TComponent : Component
{
    protected TComponent Component => (ControllerContext.ActionDescriptor.Properties[nameof(Component)] as TComponent)!;

    [HttpGet("config")]
    [EndpointSummary("Component Details")]
    public IActionResult GetDetails() => Ok(Component);
}