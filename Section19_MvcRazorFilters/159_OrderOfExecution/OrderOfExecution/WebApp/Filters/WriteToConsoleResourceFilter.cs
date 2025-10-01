using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.Helpers;

namespace WebApp.Filters;

//[AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class WriteToConsoleResourceFilter : Attribute, IResourceFilter, IOrderedFilter
{
    public string? Description { get; set; }

    public int Order { get; set; }

    public WriteToConsoleResourceFilter()
    {
        this.Description = "Global";
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        Console.WriteLine($"[[{++FilterOrderHelper.OrderCounter}]] | Executing | Order=[{Order}] | Description=[{Description}] | Action=[{context.ActionDescriptor.DisplayName}]");
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        Console.WriteLine($"[[{++FilterOrderHelper.OrderCounter}]] | Executed | Order=[{Order}] | Description=[{Description}] | Action=[{context.ActionDescriptor.DisplayName}]");
    }        
}
