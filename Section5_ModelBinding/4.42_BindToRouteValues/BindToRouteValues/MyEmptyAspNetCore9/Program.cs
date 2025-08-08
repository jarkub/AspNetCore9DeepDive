using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseRouting();

/*
// This is "implicit" binding

app.MapGet("/employees/{id:int}", (int id) =>
{
    // Get a particular employee's information
    var employee = EmployeesRepository.GetEmployeeById(id);
    if (employee is null)
    {
        return Results.NotFound("Employee not found.");
    }
    return Results.Ok(employee);
});
*/

/*
// This is "Minimal API", no longer directly using HttpContext, 
// returning a Model will result in json serialization

 This works
 Note that the parameter name must exactly match the name in the route
app.MapGet("/employees/{id:int}", (int id) => 
{
    var employee = EmployeesRepository.GetEmployeeById(id);
    return employee;
});
*/

/*
// We can be explicit about where to find the parameter with [FromRoute]
 app.MapGet("/employees/{id:int}", ([FromRoute] int id) => {});
*/

/*
// If we want to use a different name for the parameter we can
// specify its name in the route and provide a different name to use in code

app.MapGet("/employees/{id:int}", ([FromRoute(Name = "id")] int identityNumber) =>
{
    // Get a particular employee's information
    var employee = EmployeesRepository.GetEmployeeById(identityNumber);

    return employee;
});
*/

// If the route value is optional
app.MapGet("/employees/{id:int}", ([FromRoute(Name = "id")] int? identityNumber) =>
{
    
    if (identityNumber.HasValue)
    {
        // Get a particular employee's information
        var employee = EmployeesRepository.GetEmployeeById(identityNumber.Value);

        return employee; 
    }
    else
    {
        return null;
    }
});

app.Run();










