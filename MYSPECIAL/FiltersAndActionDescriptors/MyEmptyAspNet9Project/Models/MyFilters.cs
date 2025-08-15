using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Security.Claims;

namespace WebApi.Filters;

/*    Custom filters can be used to implement cross-cutting concerns such as logging, authentication, authorization, caching, etc.
    Filters can be applied globally, at the controller level, or at the action level.
    Filters can implement one or more of the following interfaces:
    - IAuthorizationFilter: For authorization logic.
    - IActionFilter: For action execution logic.
    - IResultFilter: For result processing logic.
    - IExceptionFilter: For exception handling logic.

    In ASP.NET Core, filters are executed in a specific order:
    1. Authorization filters
    2. Resource filters
    3. Action filters
    4. Result filters
    5. Exception filters

    I<something>Filter:
    Allows filter to be added as middleware

    Attribute:
    Allows filter to be used for an individual Controller or Action method

*/
public class MyAuthorizationFilter : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var actionDescriptor = context.ActionDescriptor;
        /*
            Inside the OnAuthorization method, you have access to the AuthorizationFilterContext which provides information about the current request, including: 
             - context.HttpContext: The current HTTP context.
             - context.ActionDescriptor: Information about the action being executed.
             - context.ModelState: The current model state.
             - context.Result: You can set this to redirect, return an error, etc.
            Use this information to implement your authorization logic. For example: 
             - Check if the user is authenticated.
             - Check if the user has the required role or permissions.
             - Check if the user has access to the specific resource.
        */
        // You can now access properties of the actionDescriptor,
        // such as its ActionName, ControllerName, or custom attributes.
        Console.WriteLine($"{nameof(context.HttpContext)}: {context.HttpContext}");
        Console.WriteLine($"{nameof(context.ActionDescriptor)}: {context.ActionDescriptor}");
        Console.WriteLine($"{nameof(context.ModelState)}: {context.ModelState}");
        Console.WriteLine($"{nameof(context.Result)}: {context.Result}");
        Console.WriteLine($"Action Descriptor Type: {actionDescriptor.GetType()}");

        Console.WriteLine("------------------------------------------------");
        WebApi.MyConsole.WriteObject(context.ActionDescriptor);
        Console.WriteLine("------------------------------------------------");
        //context.ActionDescriptor
        //context.ModelState
        //context.Result

        string? controllerName = null;
        string? actionName = null;
        Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor? controllerActionDescriptor = null;
        Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor? abstractActionDescriptor = null;
        if (actionDescriptor is Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor)
        {
            abstractActionDescriptor = (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)actionDescriptor;
            Console.WriteLine($"actionDescriptor=[{abstractActionDescriptor}], Type=[{abstractActionDescriptor?.GetType()}]");
        }

        if (actionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)
        {
            controllerActionDescriptor = actionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
            Console.WriteLine($"actionDescriptor=[{controllerActionDescriptor}], Type=[{controllerActionDescriptor?.GetType()}]");

            controllerName = controllerActionDescriptor?.ControllerName;
            actionName = controllerActionDescriptor?.ActionName;

            var controllerDisplayName = controllerActionDescriptor?.DisplayName;
            var controllerTypeInfo = controllerActionDescriptor?.ControllerTypeInfo;
            var controllerMethodInfo = controllerActionDescriptor?.MethodInfo;
            var controllerAssembly = controllerActionDescriptor?.ControllerTypeInfo.Assembly;
            //var typeName = Microsoft.Extensions.Internal.TypeNameHelper.GetTypeDisplayName(controllerActionDescriptor?.ControllerTypeInfo);
            // Access other properties specific to ControllerActionDescriptor
        }

        Console.WriteLine();
    }
}

public class MyGlobalActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Code to run before the action method executes
        Console.WriteLine("Action is executing...");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Code to run after the action method executes
        Console.WriteLine("Action has executed.");
    }
}

public class CustomLogActionFilter : ActionFilterAttribute
{
    private Stopwatch _stopwatch;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _stopwatch = Stopwatch.StartNew();
        Debug.WriteLine($"Executing action: {context.ActionDescriptor.DisplayName}");
        base.OnActionExecuting(context);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        _stopwatch.Stop();
        Debug.WriteLine($"Executed action: {context.ActionDescriptor.DisplayName} in {_stopwatch.ElapsedMilliseconds}ms");
        base.OnActionExecuted(context);
    }
}

public class RoleAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    /*
    // Usage example for an Action:
    [RoleAuthorize("Admin")]
    public IActionResult AdminOnlyAction()
    {
        // Action logic here
        return Ok();
    }
    // Usage example for a controller:
    [Route("api/[controller]")]
    [ApiController]
    */
    private readonly string _requiredRole;

    public RoleAuthorizeAttribute(string requiredRole)
    {
        _requiredRole = requiredRole;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!context.HttpContext.User.IsInRole(_requiredRole))
        {
            context.Result = new ForbidResult();
            return;
        }
    }
}
