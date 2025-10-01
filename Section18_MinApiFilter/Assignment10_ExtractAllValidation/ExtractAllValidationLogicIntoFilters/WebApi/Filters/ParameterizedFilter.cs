
using WebApi.Models;

namespace WebApi.Filters
{
    public class ParameterizedFilter : IEndpointFilter
    {
        private readonly string _customMessage;

        public ParameterizedFilter(string customMessage)
        {
            _customMessage = customMessage;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ParameterizedFilter>>();
            logger.LogInformation($"Parameterized filter says: {_customMessage}");

            return await next(context);
        }
    }

}
