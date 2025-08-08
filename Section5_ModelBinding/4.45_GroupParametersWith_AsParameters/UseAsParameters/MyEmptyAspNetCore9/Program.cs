using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Xml.Linq;
using UseAsParameters.AsParametersStructs;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseRouting();

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

/*
// http://localhost:5007/employees/1?name=Josephine => BadHttpRequestException: Required parameter "string position" was not provided from header
// http://localhost:5007/employees/1                =>  BadHttpRequestException: Required parameter "string name" was not provided from query string.
app.MapGet("/employees/{id:int}", (int id, [FromQuery] string name, [FromHeaderAttribute] string position) =>
{
    // Get a particular employee's information
    var employee = EmployeesRepository.GetEmployeeById(id);
    // Update the employee's information with the provided parameters
    if (employee is not null)
    {
        employee.Name = name;
        employee.Position = position;
        return Results.Json(employee);
    }

    // Using an anonymous object
    var data = new
    {
        Message = $"Employee with Id={id} Not Found"
    };

    // Using a model
    //employee = new Employee(id, "", "", 0.0);
    //var data = new
    //{
    //    Message = $"Employee Not Found",
    //    Employee = employee
    //};

    return Results.Json(data);
});
*/

// Equivalent to
// app.MapGet("/employees/{id:int}", (int id, [FromQuery] string name, [FromHeaderAttribute] string position) =>
app.MapGet("/employees/{id:int}", ([AsParameters] GetEmployeeParameterStruct param) =>
{
    // Get a particular employee's information
    var employee = EmployeesRepository.GetEmployeeById(param.Id);
    // Update the employee's information with the provided parameters
    if (employee is not null)
    {
        employee.Name = param.Name;
        employee.Position = param.Position;
        return Results.Json(employee);
    }

    // Using an anonymous object
    var data = new
    {
        Message = $"Employee with Id={param.Id} Not Found"
    };

    return Results.Json(data);
});


app.Run();


