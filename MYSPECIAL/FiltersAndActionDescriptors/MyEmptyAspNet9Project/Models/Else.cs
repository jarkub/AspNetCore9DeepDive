using System.Reflection;
using System.Web.Mvc;

namespace WebApi.Models;

public static class Else
{
    public static string ClassGet<T>(this T ctrl) where T : Microsoft.AspNetCore.Mvc.Controller
    {//4
        string actionName = "Index";

        var controllerDescriptor = new System.Web.Mvc.ReflectedControllerDescriptor(ctrl.GetType());
        var actionDescriptor = new System.Web.Mvc.ReflectedActionDescriptor(typeof(T).GetMethod(actionName), actionName, controllerDescriptor);
        var ctrlName = ctrl.GetType().Name; //.Replace("Controller", string.Empty);

        var atts = GetAttributes(ctrlName, actionName);

        return ctrlName;
    }

    public static IEnumerable<Attribute> GetAttributes(string controllerName, string actionName)
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();
        var controllers = types.Where(t => (t.Name == controllerName));
        var action = controllers.SelectMany(type => type.GetMethods().Where(a => a.Name == actionName)).FirstOrDefault();
        var attrs1 = action.GetCustomAttributes(true).OfType<Attribute>();
        var attrs2 = action.GetCustomAttributes<Attribute>(true);
        return attrs1.Concat(attrs2);
    }
}