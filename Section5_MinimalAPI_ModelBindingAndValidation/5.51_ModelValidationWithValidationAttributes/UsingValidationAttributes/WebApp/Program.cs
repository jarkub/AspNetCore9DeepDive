using WebApp.Models;

using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();


// Minimal API does not enforce data annotation validation by default, unlike MVC and Razor pages.
// We must add a 3rd pary package:
// MinimalApis.Extensions 0.11.0
// https://github.com/DamianEdwards/MinimalApis.Extensions
app.MapPost("/employees", (Employee employee) =>
{
    EmployeesRepository.AddEmployee(employee);
    return "Employee is added successfully.";
}).WithParameterValidation()
  //.WithName("add-employee")
  //.WithTags("Employee")
  //.Produces<string>(201)
  //.ProducesValidationProblem(400)
  ;
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