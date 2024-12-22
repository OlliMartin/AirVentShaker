using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Oma.WndwCtrl.Api.Controllers.Components;
using Oma.WndwCtrl.Configuration.Model;
using Oma.WndwCtrl.Core.Model;

namespace Oma.WndwCtrl.Api.Conventions;

public class ComponentApplicationConvention(ComponentConfigurationAccessor configurationAccessor)
    : IApplicationModelConvention
{
    private readonly ComponentConfigurationAccessor _configurationAccessor = configurationAccessor;

    public void Apply(ApplicationModel application)
    {
        ControllerModel? sensorBase = application.Controllers.FirstOrDefault(c => c.ControllerType == typeof(SensorController));
        
        if (sensorBase is null)
        {
            throw new InvalidOperationException("Could not find sensor controller");
        }
        
        application.Controllers.Remove(sensorBase);
        
        foreach (var component in _configurationAccessor.Configuration.Components)
        {
            if (component.Value is Sensor sensorComponent)
            {
                ControllerModel controllerModel = new(sensorBase)
                {
                    ControllerName = component.Key
                };

                foreach (var selector in controllerModel.Selectors)
                {
                    if (selector.AttributeRouteModel?.Template is not null)
                    {
                        selector.AttributeRouteModel.Template = selector.AttributeRouteModel.Template
                            .Replace("{componentName}", component.Key);
                    }
                    
                    RouteAttribute old = selector.EndpointMetadata.OfType<RouteAttribute>().Single();
                    selector.EndpointMetadata.Remove(old);
                    
                    RouteAttribute newAttr = new(old.Template.Replace("{componentName}", component.Key));
                    selector.EndpointMetadata.Add(newAttr);
                }
                
                controllerModel.RouteValues.Add("componentName", component.Key);
                controllerModel.Properties.Add("sensor", sensorComponent);
                
                application.Controllers.Add(controllerModel);
            }
            else
            {
                throw new InvalidOperationException($"Component type {component.Value.GetType().FullName} not supported.");
            }
        }
    }
}