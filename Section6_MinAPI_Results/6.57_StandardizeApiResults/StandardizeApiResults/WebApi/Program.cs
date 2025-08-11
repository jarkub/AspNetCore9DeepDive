using System.Text;
using System.Text.Json;
using WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddProblemDetails();

var app = builder.Build();

//if (!app.Environment.IsDevelopment())
//{
//app.UseExceptionHandler();
//}

//app.UseStatusCodePages();

app.MapGet("/", () =>
{
    StringBuilder sb = new StringBuilder();
    sb.AppendLine("<p>Welcome to the Employee Management API!</p>");
    sb.AppendLine("<p>Available endpoints:</p>");
    sb.AppendLine("<ol>");
    sb.AppendLine("<li><a href='/xyz'>GET /xyz - Endpoint doesn't exist</a></li>");
    sb.AppendLine("<li><a href='/employees'>GET /employees - Get all employees</a></li>");
    sb.AppendLine("<li><a href='/employees/1'>GET /employees/{id} - Get valid employee by ID</a></li>");
    sb.AppendLine("<li><a href='/employees/12'>GET /employees/{id} - Get invalid employee by ID</a></li>");
    sb.AppendLine("<li>POST /employees - Add a new employee</li>");
    //sb.AppendLine("PUT /employees/{id} - Update an existing employee");
    //sb.AppendLine("DELETE /employees/{id} - Delete an employee");
    //sb.AppendLine("Use the Swagger UI for more details.");
    //sb.AppendLine("Swagger UI: /swagger/index.html");
    //sb.AppendLine("API Documentation: /docs");
    //sb.AppendLine("OpenAPI Specification: /openapi.json");
    //sb.AppendLine("Press Ctrl+C to stop the server.");
    sb.AppendLine("</ol>");
    return Results.Content(sb.ToString(), "text/html");
}).WithName("GetRoot");

app.MapGet("/ThrowException", () =>
{
    throw new InvalidOperationException("Oops!");
}).WithName("ThrowException");

app.MapGet("/employees", () =>
{
    var employees = EmployeesRepository.GetEmployees();

    return TypedResults.Ok(employees);
}).WithName("GetAllEmployees");

app.MapGet("/employees/{id:int}", (int id) =>
{
    var employee = EmployeesRepository.GetEmployeeById(id);

    return TypedResults.Ok(employee);
}).WithName("GetEmployeeById");

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

})
.WithName("AddEmployee")
.WithParameterValidation();

app.Run();
