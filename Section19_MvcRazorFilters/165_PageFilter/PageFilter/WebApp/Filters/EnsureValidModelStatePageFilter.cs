using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.Helpers;
using WebApp.Models;

namespace WebApp.Filters
{
    public class EnsureValidModelStatePageFilter : /*Attribute,*/ IPageFilter
    {
        private readonly ILogger<EnsureEmployeeExistsPageFilter> _logger;
        public EnsureValidModelStatePageFilter(ILogger<EnsureEmployeeExistsPageFilter> logger)
        {
            _logger = logger;
        }
        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            _logger.LogCritical("OnPageHandlerExecuted executed.");
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            _logger.LogCritical("OnPageHandlerExecuting executed.");
            if (!context.ModelState.IsValid)
            {
                var errors = ModelStateHelper.GetErrors(context.ModelState);                
                context.Result = new RedirectToPageResult("/Error", new { errors });
            }
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            _logger.LogCritical("OnPageHandlerSelected executed.");
        }
    }
}
