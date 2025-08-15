using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WebApi.Models;
using static System.Net.Mime.MediaTypeNames;
var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddProblemDetails();

var app = builder.Build();

Console.WriteLine("***** FROM: builder.Services.AddProblemDetails() *****");
Console.WriteLine("1. services.TryAddSingleton<IProblemDetailsService, ProblemDetailsService>()");
var ipds = app.Services.GetService<IProblemDetailsService>();
Console.WriteLine(ipds);
Console.WriteLine();

Console.WriteLine("2. services.TryAddEnumerable(ServiceDescriptor.Singleton<IProblemDetailsWriter, DefaultProblemDetailsWriter>())");
var ipdw = app.Services.GetService<IProblemDetailsWriter>();
Console.WriteLine(ipdw);
Console.WriteLine();

Console.WriteLine("3. services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<JsonOptions>, ProblemDetailsJsonOptionsSetup>())");
var icojo = app.Services.GetService<Microsoft.Extensions.Options.IConfigureOptions<Microsoft.AspNetCore.Http.Json.JsonOptions>>();
Console.WriteLine(icojo);
Console.WriteLine();
Console.WriteLine("******************************************************");

/*
Currently you can't disable DeveloperExceptionPageMiddleware in minimal hosting model for development environment since it is set up without any options to configure. 
So options are:
1. Use/switch back to generic host model (the one with Startups) and skip the UseDeveloperExceptionPage call.
2. Do not use development environment
3. Setup custom exception handling. DeveloperExceptionPageMiddleware relies on the fact that exception was not handled later down the pipeline so 
adding custom exception handler right after the building the app should do the trick:
*/
app.Use(async (context, func) =>
{
    try
    {
        await func();
    }
    catch (Exception e)
    {
        context.Response.Clear();

        // Preserve the status code that would have been written by the server automatically when a BadHttpRequestException is thrown.
        if (e is BadHttpRequestException badHttpRequestException)
        {
            context.Response.StatusCode = badHttpRequestException.StatusCode;
        }
        else
        {
            context.Response.StatusCode = 500;
        }
        // log, write custom response and so on...
    }
});

//app.UseExceptionHandler();

//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler();
//}
//app.UseMiddleware<DeveloperExceptionPageMiddleware>();
//app.UseMiddleware<StatusCodePagesMiddleware>();
//app.UseStatusCodePages();

app.MapGet("/ThrowException", (HttpContext context) =>
{
    var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();

    throw new InvalidOperationException("This is a test exception.");
}); //.WithName("ThrowException");

app.MapGet("/", () => "Hello World!");

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
