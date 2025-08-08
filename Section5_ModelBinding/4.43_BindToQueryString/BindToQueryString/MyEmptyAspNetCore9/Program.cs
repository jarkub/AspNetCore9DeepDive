using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApp.Models;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseRouting();

// app.MapGet("/employees", (int id) =>
/* 
// If a parameter is not specified in route but is in the handler the default implicit binding will look in the query string
// Equivalent to 
// app.MapGet("/employees", ([FromQuery] int id) =>
app.MapGet("/employees", (int id) =>
{
    var employee = EmployeesRepository.GetEmployeeById(id);

    return employee;
});
*/

// app.MapGet("/employees/{id:int}", (int id) =>
/* 
// If we use both url parameter and (default) query string, the route parameter will be required and the query string ignored
// http://localhost:5007/employees?id=1         // 404
// http://localhost:5007/employees/3            // returns employee 3
// http://localhost:5007/employees/3?id=1       // returns employee 3
app.MapGet("/employees/{id:int}", (int id) =>
{
    var employee = EmployeesRepository.GetEmployeeById(id);

    return employee;
});
*/

// app.MapGet("/employees", ([FromQuery] int id) =>
/*
app.MapGet("/employees", ([FromQuery] int id) =>
{
    var employee = EmployeesRepository.GetEmployeeById(id);

    return employee;
});
*/

// app.MapGet("/employees", ([FromQuery(Name = "id")] int identityNumber) =>
/*
// http://localhost:5007/employees results in Required parameter error
app.MapGet("/employees", ([FromQuery(Name = "id")] int identityNumber) =>
{
    var employee = EmployeesRepository.GetEmployeeById(identityNumber);

    return employee;
});
*/

// app.MapGet("/employees", ([FromQuery(Name = "id")] int? identityNumber) =>

// Since a query string is always optional (for a url to be valid) its best to write it explicitly
// http://localhost:5007/employees returns null
app.MapGet("/employees", ([FromQuery(Name = "id")] int? identityNumber) =>
{
    if (identityNumber.HasValue)
    {
        // Get a particular employee's information
        var employee = EmployeesRepository.GetEmployeeById(identityNumber.Value);

        return employee;
    }

    return null;
});


app.Run();
