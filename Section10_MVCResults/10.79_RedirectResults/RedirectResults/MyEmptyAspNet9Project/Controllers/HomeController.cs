using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

using System.Diagnostics;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Text;


namespace WebApp.Controllers
{
    //[CustomLogActionFilter]
    public class HomeController : Controller
    {
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider, IWebHostEnvironment webHostEnvironment)
        {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
            _webHostEnvironment = webHostEnvironment;
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
                Debug.WriteLine($"actionDescriptor=[{actionDescriptor}], Type=[{actionDescriptor.GetType()}]");

                if (actionDescriptor is Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor )
                {
                    abstractActionDescriptor = (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)actionDescriptor;
                    Debug.WriteLine($"actionDescriptor=[{abstractActionDescriptor}], Type=[{abstractActionDescriptor?.GetType()}]");
                }
                
                
                if (actionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor )
                {
                    controllerActionDescriptor = actionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
                    Debug.WriteLine($"actionDescriptor=[{controllerActionDescriptor}], Type=[{controllerActionDescriptor?.GetType()}]");

                    controllerName = controllerActionDescriptor?.ControllerName;
                    actionName = controllerActionDescriptor?.ActionName;
                    // Access other properties specific to ControllerActionDescriptor
                }

                Debug.WriteLine("");
            }

            var types = Assembly.GetExecutingAssembly().GetTypes();
            var controllers = types.Where(t => (t.Name == controllerName + "Controller"));
            var action = controllers.SelectMany(type => type.GetMethods().Where(a => a.Name == actionName)).FirstOrDefault();
            var attrs = action?.GetCustomAttributes(true).OfType<Attribute>();

            // Static Files
            string wwwrootPath = _webHostEnvironment.WebRootPath;
            string[] filesInWwwroot = Directory.GetFiles(wwwrootPath);
            StringBuilder linksToStaticFiles = new();
            string template = "<li><a href=\"/{0}\">{0}</a></li>\r\n";
            foreach (string filePath in filesInWwwroot)
            {
                // Process each file (e.g., read content, get file info)
                string fileName = Path.GetFileName(filePath);
                linksToStaticFiles.AppendFormat(template, fileName);
                //Console.WriteLine($"Path: {filePath}, File: {fileName}");
            }
            string index =
$"""
                
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <title>Turd Ferguson</title>
</head>
<body>
<ul>
    <li>
        <a href="/Home/Index">Home</a>
    </li>
    <li>
        <a href="/Redirect/Index">RedirectController</a>
        <ul>
            <li><a href="/Redirect/RedirectToActionResult/1">RedirectToActionResult/1</a></li>
            <li><a href="/Redirect/LocalRedirectResult/1">LocalRedirectResult/1</a></li>
            <li><a href="/Redirect/RedirectResult/1">RedirectResult/1</a></li>
            <li><a href="/Redirect/PermanentRedirect/1">PermanentRedirect/1</a></li>
            <li><a href="/Redirect/TemporaryRedirect/1">TemporaryRedirect/1</a></li>
            <li><a href="/Redirect/LocalRedirect/1">LocalRedirect/1</a></li>
        </ul>
    </li>
    <li>
        <a href="/Employees/Index">EmployeesController</a>
        <ul>
            <li><a href="/Employees/GetEmployeesByDepartment/1">GetEmployeesByDepartment/1</a></li>
        </ul>
    </li>
</ul>
</body>
</html>                
""";
            return Content(index, MediaTypeNames.Text.Html);
        }
    }
}
