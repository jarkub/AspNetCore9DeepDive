using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace WebApi.Models;

public class Something : DefaultHttpControllerSelector
{
    public Something(HttpConfiguration configuration)
        : base(configuration)
    {
    }
    public override IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
    {
        // Custom logic to get the controller mapping
        return base.GetControllerMapping();
    }
    public override string GetControllerName(HttpRequestMessage request)
    {
        // Custom logic to determine the controller name
        return base.GetControllerName(request);
    }
}

public class MyControllerSelector : IHttpControllerSelector
{
    public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
    {
        throw new NotImplementedException();
    }

    public HttpControllerDescriptor SelectController(HttpRequestMessage request)
    {
        throw new NotImplementedException();
    }
}