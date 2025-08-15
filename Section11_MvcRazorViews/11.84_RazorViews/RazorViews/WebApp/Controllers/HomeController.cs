using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return new ViewResult { ViewName = "Index" };
            //return View("NotIndex");
            //return LocalRedirect("~/Departments");
            //Content("Welcome to department management.");
        }
    }
}
