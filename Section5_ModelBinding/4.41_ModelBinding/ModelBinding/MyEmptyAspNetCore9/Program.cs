using Assignment3.Models;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseRouting();

// Works a bit backwards. Route handler sees it needs a parameter of type int, named id.
// The flow looks like the parameter in hte URL is bound to the parameter in the endpoint method.
// It actually works "backwards". The endpoint sees it needs a parameter so it looks at the URL and
// searches the data source, the HttpContext, to find the value of that parameter and extracts it, binding it to itself.

//app.MapGet("/employees/{id:int}", async (HttpContext context) =>
app.MapGet("/employees/{id:int}", async (HttpContext context, int id) =>
{
    //context.Response.ContentType = "text/html";
    //var id = context.Request.RouteValues["id"]!.ToString();
    //var employeeId = int.Parse(id!);
    var employee = EmployeesRepository.GetEmployees().Where(e => e.Id == id).FirstOrDefault();
    if (employee is not null)
    {
        await context.Response.WriteAsync("<h1>Employees</h1>");
        await context.Response.WriteAsync($"<p>{employee.Id}: {employee.Name} - {employee.Position} (${employee.Salary})</p>");
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.NotFound; // 404;
        await context.Response.WriteAsync("<h1>Employees</h1>");
        await context.Response.WriteAsync($"<p>Employee with Id={id} not found</p>");
    }
});


app.Run();