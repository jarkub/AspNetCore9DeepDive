

using System.Text.Json;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseRouting();

// MINIMAL API: BODY MUST BE JSON
// MVC or Razor Pages you can choose the serializer
// When binding with body we generally want to bind with a complex type
app.MapPost("/employees", (Employee employee) =>
{
    if (employee is null || employee.Id <= 0)
    {
        return "Employee is not provided or is not valid.";
    }

    EmployeesRepository.AddEmployee(employee);
    return "Employee added successfully.";

});

app.MapGet("/employees", async (HttpContext context) =>
{
    // Get all of the employees' information
    var employees = EmployeesRepository.GetEmployees();

    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync("<h2>Employees</h2>");
    await context.Response.WriteAsync("<ul>");
    foreach (var employee in employees)
    {
        await context.Response.WriteAsync($"<li><b>{employee.Name}</b>: {employee.Position}</li>");
    }
    await context.Response.WriteAsync("</ul>");

});

app.Run();






