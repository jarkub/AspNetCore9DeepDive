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
            });

            app.MapGet("/employees", (IEmployeesRepository employeesRepository) =>
            {

                Console.WriteLine("Execution of the endpoint handler.");
                var employees = employeesRepository.GetEmployees();

                return TypedResults.Ok(employees);
            }).AddEndpointFilter(async (context, next) =>
            {
                Console.WriteLine("Before filter A");
                var result = await next(context);
                Console.WriteLine("After filter A");

                return result;
            }).AddEndpointFilter(async (context, next) =>
            {
                Console.WriteLine("Before filter B");
                var result = await next(context);
                Console.WriteLine("After filter B");

                return result;
            }).AddEndpointFilter(async (context, next) =>
            {
                Console.WriteLine("Before filter C");
                var result = await next(context);
                Console.WriteLine("After filter C");

                return result;
            });

            app.MapGet("/employees/{id:int}", (int id, IEmployeesRepository employeesRepository) =>
            {
                var employee = employeesRepository.GetEmployeeById(id);
                return employee is not null
                    ? TypedResults.Ok(employee)
                    : Microsoft.AspNetCore.Http.Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                        {"id", new[] { $"Employee with the id {id} doesn't exist." } }
                    },
                    statusCode: 404);
            });

            app.MapPost("/employees", (Employee employee, IEmployeesRepository employeesRepository) =>
            {
                if (employee is null || employee.Id < 0)
                {
                    return Microsoft.AspNetCore.Http.Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                        {"id", new[] { "Employee is not provided or is not valid." } }
                    },
                    statusCode: 400);
                }

                employeesRepository.AddEmployee(employee);
                return TypedResults.Created($"/employees/{employee.Id}", employee);

            }).WithParameterValidation();

            app.MapPut("/employees/{id:int}", (int id, Employee employee, IEmployeesRepository employeesRepository) =>
            {
                return employeesRepository.UpdateEmployee(employee)
                    ? TypedResults.NoContent()
                    : Microsoft.AspNetCore.Http.Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                        {"id", new[] { "Employee doesn't exist." } }
                    },
                    statusCode: 404);
            }).WithParameterValidation()            
            .AddEndpointFilter<EmployeeUpdateFilter>();

            app.MapDelete("/employees/{id:int}", (int id, IEmployeesRepository employeesRepository) =>
            {
                var employee = employeesRepository.GetEmployeeById(id);

                return employeesRepository.DeleteEmployee(employee)
                    ? TypedResults.Ok(employee)
                    : Microsoft.AspNetCore.Http.Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                        {"id", new[] { $"Employee with the id {id} doesn't exist." } }
                    },
                    statusCode: 404);
            });
        }
    }
}
