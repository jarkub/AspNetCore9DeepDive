using WebApi.Filters;
using WebApi.Models;
using WebApi.Results;

using static System.Net.Mime.MediaTypeNames;

namespace WebApi.Endpoints
{
    public static class EmployeeEndpoints
    {
        public static void MapEmployeeEndpoints(this WebApplication app)
        {
            app.MapGet("/", HtmlResult () =>
            {
                string html = "<h2>Welcome to our API</h2> Our API is used to learn ASP.NET CORE.";

                return new HtmlResult(html);
            })                
                .AddEndpointFilter((context, next) =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                        .CreateLogger("Custom inline endpoint filter");
                    logger.LogInformation("Custom inline endpoint filter says: Hello from inline filter!");
                    return next(context);
                })
                .AddEndpointFilter((context, next) =>
                   new ParameterizedFilter("This is a custom parameter from the factory").InvokeAsync(context, next)
                );



            app.MapGet("/employees", (IEmployeesRepository employeesRepository) =>
            {
                var employees = employeesRepository.GetEmployees();

                return TypedResults.Ok(employees);
            });

            app.MapGet("/employees/{id:int}", (int id, IEmployeesRepository employeesRepository) =>
            {
                var employee = employeesRepository.GetEmployeeById(id);
                return TypedResults.Ok(employee);
            })
                .AddEndpointFilter<EnsureEmployeeExistsOrNotFilter>()
                .AddEndpointFilter<EnsureEmployeeExistsFilter>()
                ;

            app.MapPost("/employees", (Employee employee, IEmployeesRepository employeesRepository) =>
            {
                employeesRepository.AddEmployee(employee);
                return TypedResults.Created($"/employees/{employee.Id}", employee);

            }).WithParameterValidation()
            .AddEndpointFilter((context, next) =>
                   new EnsureEmployeeExistsOrNotFilter(true).InvokeAsync(context, next)
                )
            .AddEndpointFilter<EmployeeCreateFilter>();

            app.MapPut("/employees/{id:int}", (int id, Employee employee, IEmployeesRepository employeesRepository) =>
            {
                employeesRepository.UpdateEmployee(employee);
                return TypedResults.NoContent();
            }).WithParameterValidation()
            .AddEndpointFilter<EnsureEmployeeExistsFilter>()
            .AddEndpointFilter<EmployeeUpdateFilter>();

            app.MapDelete("/employees/{id:int}", (int id, IEmployeesRepository employeesRepository) =>
            {
                var employee = employeesRepository.GetEmployeeById(id);
                employeesRepository.DeleteEmployee(employee);

                return TypedResults.Ok(employee);
            }).AddEndpointFilter<EnsureEmployeeExistsFilter>();
        }
    }
}
