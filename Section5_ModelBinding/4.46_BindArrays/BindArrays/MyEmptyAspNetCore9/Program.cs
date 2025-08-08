using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseRouting();

app.MapGet("/Employees", async (HttpContext context) =>
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

app.MapGet("/EmployeesFromQuery", ([FromQuery(Name = "id")] int[] ids) =>
{
    var employees = EmployeesRepository.GetEmployees();
    var emps = employees.Where(x => ids.Contains(x.Id)).ToList();

    return emps;
});

app.MapGet("/EmployeesFromHeader", ([FromHeader(Name = "id")] int[] ids) =>
{
    var employees = EmployeesRepository.GetEmployees();
    var emps = employees.Where(x => ids.Contains(x.Id)).ToList();

    return emps;
});


app.Run();


