using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["message1"] = "Hello from ViewData";
            ViewBag.Message2 = "Hellow from ViewBag";
            return View(this);
        }
    }
}
