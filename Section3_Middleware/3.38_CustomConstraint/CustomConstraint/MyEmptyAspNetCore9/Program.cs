
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("pos", typeof(PositionConstraint));
});

var app = builder.Build();

app.Use(async (context, next) =>
{
    await next(context);
});

app.UseRouting();

app.Use(async (context, next) =>
{
    await next(context);
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/employees", async (HttpContext context) =>
    {
        await context.Response.WriteAsync("Get employees");
    });

    endpoints.MapGet("/employees/{id}", async (HttpContext context) =>
    {
        await context.Response.WriteAsync($"Get employee named {context.Request.RouteValues["id"]}");
    });

    endpoints.MapGet("/employees/{id:int}", async (HttpContext context) =>
    {
        await context.Response.WriteAsync($"Get employee with id {context.Request.RouteValues["id"]}");
    });

    endpoints.MapGet("/employees/{category}/{position:pos}", async (HttpContext context) =>
    {
        await context.Response.WriteAsync($"Get employee in position {context.Request.RouteValues["position"]}");
    });
});

app.Run();

class PositionConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (!values.ContainsKey(routeKey)) { return false; }
        if (values[routeKey] is null) { return false; }

        if (values[routeKey] is string position)
        {
            return position.Equals("Manager", StringComparison.OrdinalIgnoreCase) ||
                   position.Equals("Developer", StringComparison.OrdinalIgnoreCase) ||
                   position.Equals("Designer", StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }
}

//if (values[routeKey] is string category && category.Equals("Position", StringComparison.OrdinalIgnoreCase))
//{
//    if (!category.Equals("Position", StringComparison.OrdinalIgnoreCase))
//    {
//        return false;
//    }

//    if (values[routeKey] is string position)
//    {
//        return position.Equals("Manager", StringComparison.OrdinalIgnoreCase) ||
//               position.Equals("Developer", StringComparison.OrdinalIgnoreCase) ||
//               position.Equals("Designer", StringComparison.OrdinalIgnoreCase);
//    }
//}