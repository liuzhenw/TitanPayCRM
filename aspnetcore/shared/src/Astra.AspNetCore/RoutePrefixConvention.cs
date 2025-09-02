using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Astra;

public class RoutePrefixConvention<TBaseController>(string prefix = "/api") : IApplicationModelConvention
{
    private readonly AttributeRouteModel _routePrefix = new(new RouteAttribute(prefix));

    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            if (controller.ControllerType.IsSubclassOf(typeof(TBaseController)) is false)
                continue;

            foreach (var selector in controller.Selectors)
            {
                selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(
                    _routePrefix, selector.AttributeRouteModel);
            }
        }
    }
}