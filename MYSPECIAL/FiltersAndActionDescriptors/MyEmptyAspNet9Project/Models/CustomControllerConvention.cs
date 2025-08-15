using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Linq;
using System.Web.Http.Dispatcher;

namespace WebApi;

public class CustomControllerConvention(/*IHttpControllerSelector _selector*/) : IApplicationModelConvention
{
    //private readonly IHttpControllerSelector selector = _selector;
    public void Apply(ApplicationModel application)
    {
        // Example: Modify controller names based on a condition
        foreach (var controller in application.Controllers)
        {
            if (controller.ControllerName.EndsWith("V1"))
            {
                controller.ControllerName = controller.ControllerName.Replace("V1", "");
            }
        }

        // Example: Add a custom property to controllers
        foreach (var controller in application.Controllers)
        {
            controller.Properties["CustomData"] = "SomeValue";
        }
    }
}