using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Filters;

public class CustomAuthorizationFilter : IAuthorizationFilter
{
    private readonly ILogger<CustomAuthorizationFilter> _logger;
    public CustomAuthorizationFilter(ILogger<CustomAuthorizationFilter> logger)
    {
        // You can use dependency injection to get services like ILogger, IConfiguration, etc.
        _logger = logger;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        _logger.LogCritical($"Result=[{context.Result}]");

        var chal = new ChallengeResult();
        _logger.LogCritical($"ChallengeResult=[{chal}]");
        var fbid = new ForbidResult();
        _logger.LogCritical($"ForbidResult=[{fbid}]");
        var nauth = new UnauthorizedResult();
        _logger.LogCritical($"UnauthorizedResult=[{nauth}]");

        var user = context.HttpContext.User;

        if (user is null || user.Identity is null || !user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult(); // Or ChallengeResult, etc.
            _logger.LogCritical($"Result=[{context.Result}]");
            return;
        }

        if (!user.HasClaim(c => c.Type == "CustomClaim" && c.Value == "CustomValue"))
        {
            context.Result = new ForbidResult(); // Or UnauthorizedResult, etc.
            _logger.LogCritical($"Result=[{context.Result}]");
            return;
        }

        // Example: Check for a specific header
        if (!context.HttpContext.Request.Headers.ContainsKey("X-Custom-Auth"))
        {
            context.Result = new ChallengeResult(); // Or ForbiddenResult, etc.
            _logger.LogCritical($"Result=[{context.Result}]");
            return;
        }

        // Example: Check a specific header value
        var authHeader = context.HttpContext.Request.Headers["X-Custom-Auth"];
        if (authHeader != "my-secret-token")
        {
            context.Result = new ForbidResult(); // Or UnauthorizedResult, etc.
            _logger.LogCritical($"Result=[{context.Result}]");
            return;
        }

        // If authorization passes, do nothing, and the request proceeds to the action.
        _logger.LogCritical($"Result=[{context.Result}]");
    }
}