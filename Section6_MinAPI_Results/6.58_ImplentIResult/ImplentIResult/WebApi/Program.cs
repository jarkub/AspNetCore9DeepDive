using System.Text.Json;
using WebApi.Models;
using WebApi.Results;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

app.UseStatusCodePages();

app.MapGet("/", HtmlResult () =>
{
    string html = "<h2>Welcome to our API</h2> Our API is used to learn ASP.NET CORE.";

    return new HtmlResult(html);
});

app.MapGet("/employees", () =>
{
    var employees = EmployeesRepository.GetEmployees();

    return TypedResults.Ok(employees);
});

app.MapPost("/employees", (Employee employee) =>
{
    if (employee is null || employee.Id < 0)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            {"id", new[] { "Employee is not provided or is not valid." } }
        });
    }

    EmployeesRepository.AddEmployee(employee);
    return TypedResults.Created($"/employees/{employee.Id}", employee);    

}).WithParameterValidation();

app.Run();
