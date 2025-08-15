using Assignment3.Models;
using Assignment3.CustomConstraints;
using System.Text.Json;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("pos", typeof(PositionConstraint));
});

var app = builder.Build();

app.UseRouting();

app.MapGet("/", async (HttpContext context) =>
{
    await context.Response.WriteAsync("Welcome to the Home Page");
});

app.MapGet("/employees", async (HttpContext context) =>
{
    // Get all employees
    context.Response.StatusCode = (int)HttpStatusCode.OK; // 200; // default
    context.Response.ContentType = "text/html";
    var employees = EmployeesRepository.GetEmployees();
    //await context.Response.WriteAsync("<p>");
    await context.Response.WriteAsync("<h1>Employees</h1>");
    await context.Response.WriteAsync("<ul>");
    foreach (var employee in employees)
    {
        await context.Response.WriteAsync($"<li>{employee.Id}: {employee.Name} - {employee.Position} (${employee.Salary})</li>");
    }
    await context.Response.WriteAsync("</ul>");
    //await context.Response.WriteAsync("</p>");

});

app.MapGet("/employees/{id:int}", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html";
    var id = context.Request.RouteValues["id"]!.ToString();
    var employeeId = int.Parse(id!);
    var employee = EmployeesRepository.GetEmployees().Where(e => e.Id == employeeId).FirstOrDefault();
    if (employee is not null)
    {
        await context.Response.WriteAsync("<h1>Employees</h1>");
        await context.Response.WriteAsync($"<p>{employee.Id}: {employee.Name} - {employee.Position} (${employee.Salary})</p>");
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.NotFound; // 404;
        await context.Response.WriteAsync("<h1>Employees</h1>");
        await context.Response.WriteAsync($"<p>Employee with Id={employeeId} not found</p>");
    }
});

app.MapGet("/employees/{category}/{position:pos}", async (HttpContext context) =>
{
    await context.Response.WriteAsync($"Get employee in position {context.Request.RouteValues["position"]}");
});

app.MapPost("/employees", async (HttpContext context) =>
{
    using var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();

    try
    {
        var employee = JsonSerializer.Deserialize<Employee>(body);
        if (employee is null || employee.Id <= 0)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400;
            return;
        }

        EmployeesRepository.AddEmployee(employee);

        context.Response.StatusCode = (int)HttpStatusCode.Created; // 201;
        //await context.Response.WriteAsync("<h1>Employees</h1>");
        //await context.Response.WriteAsync("Employee added successfully.");
        //await context.Response.WriteAsync($"<p>{employee.Id}: {employee.Name} - {employee.Position} (${employee.Salary})</p>");
        context.Response.ContentType = "app/json";
        await context.Response.WriteAsJsonAsync(employee);
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync(ex.ToString());
    }
});

app.MapPut("/employees", async (HttpContext context) =>
{
    using var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();

    try
    {
        var employee = JsonSerializer.Deserialize<Employee>(body);
        if (employee is null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound; // 404;
            await context.Response.WriteAsync("Employee not found.");
            return;
        }

        var result = EmployeesRepository.UpdateEmployee(employee);
        if (result)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NoContent; // 204;
            context.Response.ContentType = "app/json";
            await context.Response.WriteAsJsonAsync(employee);
            return;
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400;
            await context.Response.WriteAsync("Update employee failed."); 
        }

    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync(ex.ToString());
    }
});

app.MapDelete("/employees/{id:int}", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html";
    var id = context.Request.RouteValues["id"]!.ToString();
    var employeeId = int.Parse(id!);
    var employees = EmployeesRepository.GetEmployees();
    var employee = employees.Where(e => e.Id == employeeId).FirstOrDefault();
    if (employee is not null)
    {
        var result = EmployeesRepository.DeleteEmployee(employee);
        if (result)
        {
            //context.Response.StatusCode = (int)HttpStatusCode.NoContent; // 204; // Writing to the response body is invalid for responses with status code 204.
            context.Response.StatusCode = (int)HttpStatusCode.OK; // 200;
            await context.Response.WriteAsync("<h1>Employees</h1>");
            await context.Response.WriteAsync("<h1>Successfully deleted employee</h1>");
            await context.Response.WriteAsync("<ul>");
            
            foreach (var emp in employees)
            {
                await context.Response.WriteAsync($"<li>{emp.Id}: {emp.Name} - {emp.Position} (${emp.Salary})</li>");
            }
            await context.Response.WriteAsync("</ul>");
            return;
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400;
            await context.Response.WriteAsync("<h1>Employees</h1>");
            await context.Response.WriteAsync("Delete employee failed.");
        }
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.NotFound; // 404;
        await context.Response.WriteAsync("<h1>Employees</h1>");
        await context.Response.WriteAsync($"<p>Employee with Id={employeeId} not found</p>");
    }
});


app.Run();