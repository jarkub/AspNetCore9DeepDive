
using WebApi.Models;

namespace WebApi.Filters
{
    public class CombinedFilter : IEndpointFilter
    {
        private readonly string _customMessage;

        public CombinedFilter(string customMessage)
        {
            _customMessage = customMessage;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            // Access DI services inside the InvokeAsync method
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<CombinedFilter>>();
            logger.LogInformation($"The factory-configured message is: {_customMessage}");

            return await next(context);
        }
    }

}
