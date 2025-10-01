
using WebApi.Models;

namespace WebApi.Filters
{
    public class EnsureEmployeeExistsOrNotFilter : IEndpointFilter
    {
        //private readonly IEmployeesRepository employeesRepository;
        private readonly bool _not;

        public EnsureEmployeeExistsOrNotFilter(/*IEmployeesRepository employeesRepository,*/ bool not = false)
        {
            //this.employeesRepository = employeesRepository;
            _not = not;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<CombinedFilter>>();
            var employeesRepository = context.HttpContext.RequestServices.GetRequiredService<IEmployeesRepository>();
            int id = int.MinValue;
            Type? t = context.Arguments?.FirstOrDefault()?.GetType();
            if (t == typeof(Employee))
            {
                var emp = context.Arguments?.FirstOrDefault() as Employee;
                if (emp is not null)
                {
                    id = emp.Id;
                }
            }
            else
            {
                id = context.GetArgument<int>(0);
            }

            if (_not == employeesRepository.EmployeeExists(id))
            {
                return Microsoft.AspNetCore.Http.Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                        {"id", new[] { $"Employee with the id {id} {(_not ? "already exists" : "doesn't exist")}." } }
                    },
                    statusCode: 404);
            }

            return await next(context);
        }
    }
}
