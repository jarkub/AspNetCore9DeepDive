

using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/employees", () =>
{
    var employees = EmployeesRepository.GetEmployees();
    return TypedResults.Ok(employees);
});


// seems best
app.MapGet("/employees/{id:int}", (int id) =>
{
    var employee = EmployeesRepository.GetEmployeeById(id);

    if (employee is null)
    {
        //return Results.NotFound("Employee not found.");

        Dictionary<string, object?> problemDict =
            new Dictionary<string, object?>
        {
            {nameof(Employee.Id), new[] { $"Employee with Id {id} not found." } }
        };
        var problemDetails = new ProblemDetails();
        problemDetails.Extensions = problemDict;

        return Results.Problem(problemDetails);
        //return Results.ValidationProblem(
        //    new Dictionary<string, string[]>
        //{
        //    {nameof(Employee.Id), new[] { $"Employee with Id {id} not found." } }
        //});
    }

    var trOk = TypedResults.Ok(employee);
    return trOk;
});

app.MapGet("/employees/FromPath/Int/{id:int}", (int id) =>
{
    var employee = EmployeesRepository.GetEmployeeById(id);

    return TypedResults.Ok(employee);
});

//app.MapGet("/employees/FromPath/IntObj/{id:int}", (object id) =>
//{
//    int id1 = int.Parse(id.ToString());
//    var employee = EmployeesRepository.GetEmployeeById(id1);

//    return TypedResults.Ok(employee);
//});

app.MapGet("/employees/FromPath/ObjInt/{id}", (int id) =>
{

    var employee = EmployeesRepository.GetEmployeeById(id);

    return TypedResults.Ok(employee);
});

//app.MapGet("/employees/FromPath/ObjObj/{id}", (id) => // no work
app.MapGet("/employees/FromPath/ObjObj/{id}", (string id) =>
{
    int id1 = int.Parse(id.ToString());
    var employee = EmployeesRepository.GetEmployeeById(id1);

    //return TypedResults.Ok(employee); // no work
    return TypedResults.Ok(employee);
});

app.MapGet("/employees/FromContext/Obj/{id}", (HttpContext context) =>
{
    var objId = context.Request.RouteValues["id"];
    int.TryParse(objId.ToString(), out int id1);
    int id2 = int.Parse(objId.ToString());
    int? id3 = objId as int?;
    var employee = EmployeesRepository.GetEmployeeById(id1);

    return TypedResults.Ok(employee);
});

app.MapGet("/employees/FromContext/Int/{id:int}", (HttpContext context) =>
{
    var objId = context.Request.RouteValues["id"];
    //int id1 = (int)objId;
    int.TryParse(objId.ToString(), out int id1);
    int id2 = int.Parse(objId.ToString());
    int? id3 = objId as int?;
    var employee = EmployeesRepository.GetEmployeeById(id1);

    return TypedResults.Ok(employee);
});

app.MapGet("/employees/NotInPath", (HttpContext context) =>
{
    var objId = context.Request.RouteValues["id"];
    int.TryParse(objId.ToString(), out int id1);
    int id2 = int.Parse(objId.ToString());
    int? id3 = objId as int?;
    var employee = EmployeesRepository.GetEmployeeById(id1);

    return TypedResults.Ok(employee);
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






