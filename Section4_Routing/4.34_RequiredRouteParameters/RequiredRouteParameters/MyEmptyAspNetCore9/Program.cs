using System.Net.Mime;
using MyMiddleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<TimeLoggingMiddleware>();

var app = builder.Build();
var middlewareSettings = builder.Configuration.GetSection("MiddlewareSettings").Get<MiddlewareSettings>() ?? new MiddlewareSettings();

if (middlewareSettings.UseTimeLoggingMiddleware)
{
    app.UseMiddleware<TimeLoggingMiddleware>();
}

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
        context.Response.ContentType = "application/xml";
        await context.Response.WriteAsync("Get employees");
    });

    endpoints.MapGet("/employees/{id}", async (HttpContext context) =>
    {
        context.Response.ContentType = "application/xml";
        await context.Response.WriteAsync($"<p>Get the employee: {context.Request.RouteValues["id"]}</p>");
    });

    endpoints.MapPost("/employees/{id}", async (HttpContext context) =>
    {
        await context.Response.WriteAsync($"Create an employee {context.Request.RouteValues["id"]}");
    });

    endpoints.MapPut("/employees", async (HttpContext context) =>
    {
        await context.Response.WriteAsync("Update an employee");
    });

    endpoints.MapDelete("/employees/{id}", async (HttpContext context) =>
    {
        await context.Response.WriteAsync($"Delete the employee: {context.Request.RouteValues["id"]}");
    });
});

app.Run();
