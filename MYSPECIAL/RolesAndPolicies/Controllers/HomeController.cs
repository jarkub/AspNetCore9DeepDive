using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AuthMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AuthMvc.Controllers;

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

    public IActionResult Login()
    {
        return View();
    }

    [Authorize]
    public IActionResult AuthCheck()
    {
        return View();
    }

    [Authorize(Roles = "Admin,Manager")]
    public IActionResult RoleCheck()
    {
        return View();
    }

    //[Authorize(Policy = "RequireAdminRole")]
    [Authorize(Policy = "AtLeast21")]
    public IActionResult PolicyCheck()
    {
        return View();
    }

    public async Task<IActionResult> LogMeIn()
    {
        var claims = new List<Claim>();
    //{
    //    new Claim(ClaimTypes.Name, "username"),
    //    // Add other relevant claims
    //};

        // Create a ClaimsIdentity with an authentication type (e.g., CookieAuthenticationDefaults.AuthenticationScheme)
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        // Create the ClaimsPrincipal
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        // Now, claimsPrincipal.Identity.IsAuthenticated should be true
        await HttpContext.SignInAsync(claimsPrincipal);

        //var user = HttpContext.User;
        //await HttpContext.SignInAsync(
        //    CookieAuthenticationDefaults.AuthenticationScheme, user
        //);
        //CookieAuthenticationDefaults.AuthenticationScheme,
        //    await AddPolicyClaim()
        return NoContent();
    }
    public async Task<IActionResult> LogMeOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return NoContent();
    }

    public async Task<IActionResult> AddPolicyClaim()
    {
        ClaimsPrincipal principal = HttpContext.User; //await _userManager.FindByNameAsync(model.Email);
        if (principal is not null)
        {
            // Example: Add a custom claim if not already present
            if (!principal.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                //if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                //{
                //    // User is not authenticated, cannot add claims
                //    return Task.FromResult(principal);
                //}
                if (principal.Identity is not null && principal.Identity.IsAuthenticated)
                {
                    var identity = (ClaimsIdentity)principal.Identity;
                    identity.AddClaim(new Claim(ClaimTypes.DateOfBirth, DateTime.Now.AddYears(-22).ToString()));
                    await HttpContext.SignInAsync(principal);
                }
            }
        }
        return NoContent();
        //return Task.FromResult(principal ?? new ClaimsPrincipal());
    }

    public async Task<IActionResult> AddRoleClaim()
    {
        ClaimsPrincipal principal = HttpContext.User; //await _userManager.FindByNameAsync(model.Email);
        if (principal is not null)
        {
            // Example: Add a custom claim if not already present
            if (!principal.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                //if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                //{
                //    // User is not authenticated, cannot add claims
                //    return Task.FromResult(principal);
                //}
                if (principal.Identity is not null && principal.Identity.IsAuthenticated)
                {
                    var identity = (ClaimsIdentity)principal.Identity;
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                    await HttpContext.SignInAsync(principal);
                }
            }
        }
        return NoContent();
        //return Task.FromResult(principal ?? new ClaimsPrincipal());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult AccessDenied()
    {
        return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
