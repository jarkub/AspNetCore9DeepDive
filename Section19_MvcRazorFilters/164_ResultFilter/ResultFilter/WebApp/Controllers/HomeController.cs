using Microsoft.AspNetCore.Mvc;
using WebApp.Filters;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        [AddHeaderFilter(Name = "X-Frame-Options", Value = "DENY")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
