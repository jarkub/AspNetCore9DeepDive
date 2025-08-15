using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace WebApi.Models;

public class MyService
{
    private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

    public MyService(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
    {
        _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
    }

    public void DoSomethingWithActions()
    {
        foreach (var actionDescriptor in _actionDescriptorCollectionProvider.ActionDescriptors.Items)
        {
            // Process each actionDescriptor
        }
    }
}