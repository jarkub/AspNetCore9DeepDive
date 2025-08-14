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
        <a href="/JsonResults/Index">JsonResultsController</a>
        <ul>
            
            <li><a href="/JsonResults/GetAnonymousObjectById/1">GetAnonymousObjectById/1</a></li>
            <li><a href="/JsonResults/GetJsonObjectById/1">GetJsonObjectById/1</a></li>
            <li><a href="/JsonResults/GetJsonResultById/1">GetJsonResultById/1</a></li>
            <li><a href="/JsonResults/GetIActResJsonById/1">GetIActResJsonById/1</a></li>
        </ul>
    </li>
    <li>
        <a href="/FileResults/Index">FileResultsController</a>
        <ul>
            <li><a href="/download_vf">ReturnVirtualFile</a></li>
            <li><a href="/download_pf">ReturnPhysicalFile</a></li>
            <li><a href="/download_cf">ReturnContentFile</a></li>
            <li><a href="/download_file">DownloadFile</a></li>
            <li><a href="/download_file/file.txt">DownloadTextFile</a></li>
        </ul>
    </li>
    <li>
        <a href="/ContentResults/Index">ContentResultsController</a>
        <ul>
            <li><a href="/ContentResults/TextPlain">TextPlain</a></li>
            <li><a href="/ContentResults/TextHtml">TextHtml</a></li>
        </ul
    </li>
    <li>
        <a href="/">StaticFiles</a>
        <ul>
            {linksToStaticFiles.ToString()}
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
