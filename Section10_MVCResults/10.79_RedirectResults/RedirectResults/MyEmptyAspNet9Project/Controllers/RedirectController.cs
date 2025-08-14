using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    
    public class RedirectController : Controller
    {
        public string Index()
        {
            return $"This controller is {nameof(RedirectController)}";
        }

        /*
            http://localhost:5038/Redirect/RedirectToActionResult/1
        */
        public IActionResult RedirectToActionResult(int? id)
        {
            string action = nameof(EmployeesController.GetEmployeesByDepartment);
            string controllerName = nameof(EmployeesController).Replace("Controller", string.Empty);
            // Temporary redirect (302)
            // Location header = /Employees/GetEmployeesByDepartment/{id}
            return new RedirectToActionResult(action, controllerName, new { id = id });
        }

        /*
            http://localhost:5038/Redirect/LocalRedirectResult/1
        */
        public IActionResult LocalRedirectResult(int? id)
        {
            // Temporary redirect (302)
            return new LocalRedirectResult("/Employees/GetEmployeesByDepartment/" + id);
        }

        /*
            http://localhost:5038/Redirect/RedirectResult/1
        */
        public IActionResult RedirectResult()
        {
            // Temporary redirect (302)
            return new RedirectResult("http://www.google.com");
        }

        /*
            http://localhost:5038/Redirect/PermanentRedirect/1
        */
        public IActionResult PermanentRedirect()
        {
            // Permanent redirect (301)
            return RedirectPermanent("https://www.google.com");
        }

        /*
            http://localhost:5038/Redirect/TemporaryRedirect/1
        */
        public IActionResult TemporaryRedirect()
        {
            // Temporary redirect (302)
            return Redirect("https://www.google.com");
        }
    }
}
