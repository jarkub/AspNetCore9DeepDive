

using System.Text.Json;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/employees", () =>
{
    var employees = EmployeesRepository.GetEmployees();
    return TypedResults.Ok(employees);
});

app.MapPost("/employees", (Employee employee) =>
{
    if (employee is null || employee.Id == 0)
    {
        /*
        return Results.BadRequest("Employee is not provided or is not valid.");
        produces the following:
        {
            "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            "title": "One or more validation errors occurred.",
            "status": 400,
            "errors": {
                "id": [
                    "Employee is not provided or is not valid."
                ]
            }
        }
        */

        // Manually constructing the ProblemDetailsStandard
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            {nameof(Employee.Id), new[] { "Employee is not provided or is not valid." } }
        });
    }

    if (employee is null || employee.Id < 0)
    {
        return Results.BadRequest("Employee is not provided or is not valid.");
    }

    EmployeesRepository.AddEmployee(employee);
    return TypedResults.Created($"/employees/{employee.Id}", employee);

}).WithParameterValidation();

app.Run();






