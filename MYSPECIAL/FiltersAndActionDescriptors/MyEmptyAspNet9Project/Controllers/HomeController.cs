using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

using System.Net;
using System.Net.Mime;
using System.Reflection;

using WebApi.Filters;

namespace WebApp.Controllers
{
    //[CustomLogActionFilter]
    public class HomeController : Controller
    {
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
        public HomeController(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        //[MyAuthorizationFilter]
        public IActionResult Index()
        {
            string? controllerName = null;
            string? actionName = null;
            //Microsoft.AspNetCore.Mvc.Infrastructure.ActionDescriptor myActionDescriptor = null;
            Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor? controllerActionDescriptor = null;
            Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor? abstractActionDescriptor = null;
            foreach (var actionDescriptor in _actionDescriptorCollectionProvider.ActionDescriptors.Items)
            {
                Console.WriteLine($"actionDescriptor=[{actionDescriptor}], Type=[{actionDescriptor.GetType()}]");

                if (actionDescriptor is Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor )
                {
                    abstractActionDescriptor = (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)actionDescriptor;
                    Console.WriteLine($"actionDescriptor=[{abstractActionDescriptor}], Type=[{abstractActionDescriptor?.GetType()}]");
                }
                
                
                if (actionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor )
                {
                    controllerActionDescriptor = actionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
                    Console.WriteLine($"actionDescriptor=[{controllerActionDescriptor}], Type=[{controllerActionDescriptor?.GetType()}]");

                    controllerName = controllerActionDescriptor?.ControllerName;
                    actionName = controllerActionDescriptor?.ActionName;
                    // Access other properties specific to ControllerActionDescriptor
                }

                Console.WriteLine();
            }

            var types = Assembly.GetExecutingAssembly().GetTypes();
            var controllers = types.Where(t => (t.Name == controllerName + "Controller"));
            var action = controllers.SelectMany(type => type.GetMethods().Where(a => a.Name == actionName)).FirstOrDefault();
            var attrs = action?.GetCustomAttributes(true).OfType<Attribute>();


            return Content(
"""
                
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <title>Turd Ferguson</title>
</head>
<body>
<ul>
    <li><a href="/JsonResults/Index">JsonResultsController</a></li>
    <li>
        <a href="/FileResults/Index">FileResultsController</a>
        <ul>
            <li><a href="/FileResults/TextPlain">TextPlain</a></li>
            <li><a href="/FileResults/TextHtml">TextHtml</a></li>
            <li><a href="/FileResults/ReturnVirtualFile">ReturnVirtualFile</a></li>
            <li><a href="/FileResults/ReturnPhysicalFile">ReturnPhysicalFile</a></li>
        </ul>
    </li>
    <li>
        <a href="/ContentResults/Index">ContentResultsController</a>
        <ul>
            <li><a href="/ContentResults/TextPlain">TextPlain</a></li>
            <li><a href="/ContentResults/TextHtml">TextHtml</a></li>
        </ul
    </li>
</ul>
</body>
</html>                
""",
                MediaTypeNames.Text.Html);
        }
    }
}
