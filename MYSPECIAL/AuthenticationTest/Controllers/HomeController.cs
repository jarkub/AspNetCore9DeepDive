using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AuthenticationTest.Models;
using Microsoft.AspNetCore.Authorization;

namespace AuthenticationTest.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Authorize]
    public IActionResult Restricted()
    {
        return View();
    }

    [Authorize(Roles ="Wanker")]
    public IActionResult Wanker()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        return Content("/Home/Login");
    }

    [AllowAnonymous]
    public IActionResult Logout()
    {
        return Content("/Home/Logout");
    }

    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return Content("/Home/AccessDenied");
    }
}
