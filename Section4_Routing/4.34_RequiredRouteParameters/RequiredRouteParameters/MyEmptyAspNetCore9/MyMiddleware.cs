using System.Diagnostics;
using System.Globalization;

namespace MyMiddleware;

public class TimeLoggingMiddleware : IMiddleware
{
    private readonly ILogger<TimeLoggingMiddleware> _logger;

    public TimeLoggingMiddleware(ILogger<TimeLoggingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();

        await next(context);

        watch.Stop();
        _logger.Log(LogLevel.Information, $"[{context.Request.Method}, {context.Request.Path}] Time to execute: {watch.ElapsedMilliseconds} milliseconds.");
    }
}

public class MiddlewareSettings
{
    public bool UseTimeLoggingMiddleware { get; set; }
}

// This middleware sets the culture based on the query parameter "culture".
// Examples:
//  - http://ourDemoSite.com/Users/174/Details?culture=fr-FR
//  - http://ourDemoSite.com/Invoices/Details/1235267376?culture=uk
public class CultureMiddleware
{
    private readonly RequestDelegate _next;
    public readonly ILogger<CultureMiddleware> _logger;
    public CultureMiddleware(RequestDelegate next, ILogger<CultureMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var cultureQuery = context.Request.Query["culture"];
        if (!string.IsNullOrWhiteSpace(cultureQuery))
        {
            var culture = new CultureInfo(cultureQuery);

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }
        await _next(context);
    }
}