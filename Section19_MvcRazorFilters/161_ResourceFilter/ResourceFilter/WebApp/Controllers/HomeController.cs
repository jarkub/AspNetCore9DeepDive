using Microsoft.AspNetCore.Mvc;
using WebApp.Filters;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
