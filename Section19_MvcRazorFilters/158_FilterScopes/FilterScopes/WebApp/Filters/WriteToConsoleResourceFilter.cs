using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Filters
{
    public class WriteToConsoleResourceFilter : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            Console.WriteLine($"Executing {context.ActionDescriptor.DisplayName}");
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            Console.WriteLine($"Executed {context.ActionDescriptor.DisplayName}");
        }        
    }
}
