using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

public static class MyExtensions
{
    public static void MyUseExceptionHandlerPage(this WebApplication app, string path = "/Error")
    {
        ArgumentNullException.ThrowIfNull(app);
        // This is a custom extension method to handle exceptions and redirect to a specific page.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler(path);
            app.UseHsts();
        }
    }

    public static void MyUseExceptionHandlerPathFeature(this HttpContext context)
    {
        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();

        string? ExceptionMessage = exceptionHandlerPathFeature?.Error?.Message;

        if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
        {
            ExceptionMessage = "The file was not found.";
        }

        if (exceptionHandlerPathFeature?.Path == "/")
        {
            ExceptionMessage ??= string.Empty;
            ExceptionMessage += " Page: Home.";
        }
    }

    public static void MyExceptionHandlerLambda(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    // using static System.Net.Mime.MediaTypeNames;
                    context.Response.ContentType = Text.Plain;

                    await context.Response.WriteAsync("An exception was thrown.");

                    var exceptionHandlerPathFeature =
                        context.Features.Get<IExceptionHandlerPathFeature>();

                    if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
                    {
                        await context.Response.WriteAsync(" The file was not found.");
                    }

                    if (exceptionHandlerPathFeature?.Path == "/")
                    {
                        await context.Response.WriteAsync(" Page: Home.");
                    }
                });
            });

            app.UseHsts();
        }
    }

    public static void MyUseStatusCodePages(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseStatusCodePages();

    }

    public static void MyUseStatusCodePagesWithFormatString(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        // using static System.Net.Mime.MediaTypeNames;
        app.UseStatusCodePages(Text.Plain, "Status Code Page: {0}");

    }

    public static void MyUseStatusCodePagesWithWithLambda(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseStatusCodePages(async statusCodeContext =>
        {
            // using static System.Net.Mime.MediaTypeNames;
            statusCodeContext.HttpContext.Response.ContentType = Text.Plain;

            await statusCodeContext.HttpContext.Response.WriteAsync(
                $"Status Code Page: {statusCodeContext.HttpContext.Response.StatusCode}");
        });
    }

    public static void MyUseStatusCodePagesWithWithRedirects(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseStatusCodePagesWithRedirects("/StatusCode/{0}");
    }

    public static void MyUseStatusCodePagesWithWithReExecute(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");
    }
    public static void MyUseStatusCodeReExecuteFeature(this HttpContext context)
    {
        int OriginalStatusCode = context.Response.StatusCode;

        var statusCodeReExecuteFeature =
            context.Features.Get<IStatusCodeReExecuteFeature>();

        string OriginalPathAndQuery = string.Empty;

        if (statusCodeReExecuteFeature is not null)
        {
            OriginalPathAndQuery = string.Join(
                statusCodeReExecuteFeature.OriginalPathBase,
                statusCodeReExecuteFeature.OriginalPath,
                statusCodeReExecuteFeature.OriginalQueryString);
        }
    }

    public static void MyDisableStatusCodePages(this HttpContext context)
    {
        var statusCodePagesFeature =
        context.Features.Get<IStatusCodePagesFeature>();

        if (statusCodePagesFeature is not null)
        {
            statusCodePagesFeature.Enabled = false;
        }

    }

    public static void MyAccessTheException(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);
        // This is a custom extension method to access the exception handling middleware.
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (Exception ex)
            {
                // Handle the exception here, e.g., log it or modify the response.
                context.Response.StatusCode = 500; // Internal Server Error
                await context.Response.WriteAsync($"An error occurred: {ex.Message}");
            }
        });
    }

    /// <summary>
    /// Checks if the endpoint has the <see cref="SkipStatusCodePagesAttribute"/> metadata.
    /// </summary>
    /// <param name="endpoint">The endpoint to check.</param>
    /// <returns>True if the endpoint has the attribute, otherwise false.</returns>
    public static bool HasSkipStatusCodePagesMetadata(this Endpoint? endpoint)
    {
        return endpoint?.Metadata.GetMetadata<SkipStatusCodePagesAttribute>() is not null;
    }
}