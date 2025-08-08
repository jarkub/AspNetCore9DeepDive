
namespace WebApp.MiddleComponents
{
    public class MyCustomMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("My custom middleware: Before calling next\r\n");

            await next(context);

            await context.Response.WriteAsync("My custom middleware: After calling next\r\n");
        }
    }
}
