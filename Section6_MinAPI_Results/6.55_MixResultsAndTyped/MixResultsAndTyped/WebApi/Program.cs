using System.Text.Json;
using WebApi.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

/*
    Microsoft.AspNetCore.Http.HttpResults
    Ok.cs:          public sealed class Ok              : IResult, IEndpointMetadataProvider, IStatusCodeHttpResult
    OkOfT.cs:       public sealed class Ok<TValue>      : IResult, IEndpointMetadataProvider, IStatusCodeHttpResult, IValueHttpResult, IValueHttpResult<TValue>
    Created.cs:     public sealed class Created         : IResult, IEndpointMetadataProvider, IStatusCodeHttpResult, IValueHttpResult, IValueHttpResult<TValue>
    CreatedOfT.cs:  public sealed class Created<TValue> : IResult, IEndpointMetadataProvider, IStatusCodeHttpResult, IValueHttpResult, IValueHttpResult<TValue>
*/

app.MapGet("/employees", () =>
{
    var employees = EmployeesRepository.GetEmployees();
    var trOk = TypedResults.Ok(employees); // {Microsoft.AspNetCore.Http.HttpResults.Ok<WebApi.Models.Employee>}	Microsoft.AspNetCore.Http.HttpResults.Ok<WebApi.Models.Employee>
    var rOk = Results.Ok(employees); // {Microsoft.AspNetCore.Http.HttpResults.Ok<WebApi.Models.Employee>}	Microsoft.AspNetCore.Http.IResult {Microsoft.AspNetCore.Http.HttpResults.Ok<WebApi.Models.Employee>}
    var trBad = TypedResults.BadRequest("Employee is not provided or is not valid.");
    var rBad = Results.BadRequest("Employee is not provided or is not valid.");

    return trOk;
});

app.MapPost("/employees", (Employee employee) =>
{
    var trOk = TypedResults.Ok(employee); // {Microsoft.AspNetCore.Http.HttpResults.Ok<WebApi.Models.Employee>}	Microsoft.AspNetCore.Http.HttpResults.Ok<WebApi.Models.Employee>
    var rOk = Results.Ok(employee); // {Microsoft.AspNetCore.Http.HttpResults.Ok<WebApi.Models.Employee>}	Microsoft.AspNetCore.Http.IResult {Microsoft.AspNetCore.Http.HttpResults.Ok<WebApi.Models.Employee>}
    var rCreated = Results.Created(
        $"/employees/{employee.Id}", 
        employee);
    var trCreated = TypedResults.Created($"/employees/{employee.Id}", employee);
    var trBad = TypedResults.BadRequest("Employee is not provided or is not valid.");
    var rBad = Results.BadRequest("Employee is not provided or is not valid.");

    if (employee is null /*|| employee.Id <= 0*/)
    {
        // TypedResults.BadRequest => // An IResult that on execution will write an object to the response with Bad Request (400) status code.
        // Results.BadRequest => // An IResult that on execution will write a Bad Request (400) status code to the response.
        return rBad;
    }

    EmployeesRepository.AddEmployee(employee);
    //return TypedResults.Created($"/employees/{employee.Id}", employee);
    //return Results.Ok(employee);
    // TypedResults.Ok => // An IResult that on execution will write an object to the response with OK (200) status code.
    // Results.Ok => // An IResult that on execution will write an OK (200) status code to the response.

    return trCreated;

}).WithParameterValidation();

app.Run();
