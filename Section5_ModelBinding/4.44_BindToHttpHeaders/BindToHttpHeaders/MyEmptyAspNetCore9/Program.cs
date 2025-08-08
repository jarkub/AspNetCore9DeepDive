using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseRouting();


// You must use explicit bindning for headers. There are only implicit binding for url and query strings.
// http://localhost:5007/employees?id=1 => required parameter error
// http://localhost:5007/employees/1 => 404 not found
// http://localhost:5007/employees using Postman, adding header "identity"=1
/*
app.MapGet("/employees", ([FromHeader(Name = "identity")] int id) =>
{
    // Get a particular employee's information
    var employee = EmployeesRepository.GetEmployeeById(id);

    return employee;
});
*/

app.MapGet("/employees", ([FromHeader(Name = "identity")] int? id) =>
{
    if (id.HasValue)
    {
        // Get a particular employee's information
        var employee = EmployeesRepository.GetEmployeeById(id.Value);

        return employee; 
    }

    return null;
});

app.Run();
