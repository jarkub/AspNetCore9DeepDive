
namespace MvcResultTypes.MyMiddleware
{
    public class IgnoreFavicon : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Path == "/favicon.ico")
            {
                context.Response.StatusCode = 204; // No Content
                return;
            }

            await next(context);
        }
    }
}
