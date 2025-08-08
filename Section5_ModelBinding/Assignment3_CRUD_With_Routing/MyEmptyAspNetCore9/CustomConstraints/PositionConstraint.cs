namespace Assignment3.CustomConstraints;

public class PositionConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (!values.ContainsKey(routeKey)) { return false; }
        if (values[routeKey] is null) { return false; }

        if (values[routeKey] is string position)
        {
            return position.Equals("Manager", StringComparison.OrdinalIgnoreCase) ||
                   position.Equals("Developer", StringComparison.OrdinalIgnoreCase) ||
                   position.Equals("Designer", StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }
}