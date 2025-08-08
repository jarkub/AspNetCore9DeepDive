
using Microsoft.Extensions.Primitives;
using System.Text;
using System.Web;

var builder = WebApplication.CreateBuilder(args); // Creates Kestrel web server
var app = builder.Build(); // Creates web app

app.Run(async (HttpContext context) =>
{
    if (context.Request.Method == "GET")
    {
        if (context.Request.Path.StartsWithSegments("/"))
        {
            var nl = Environment.NewLine;
            await context.Response.WriteAsync($"The medthod is: {context.Request.Method}{nl}");
            await context.Response.WriteAsync($"The Url is: {context.Request.Path}{nl}");

            await context.Response.WriteAsync($"{nl}Headers:{nl}");
            foreach (var key in context.Request.Headers.Keys)
            {
                await context.Response.WriteAsync($"{key,-30}: {context.Request.Headers[key]}{nl}");
            }
        }
        else if (context.Request.Path.StartsWithSegments("/employees"))
        {
            await context.Response.WriteAsync("Employee List");
            var employees = EmployeesRepository.GetEmployees();

            foreach (var employee in employees)
            {
                await context.Response.WriteAsync($"{Environment.NewLine}Id: {employee.Id}, Name: {employee.Name}, Position: {employee.Position}, Salary: {employee.Salary}");
            }
        }
    }
});

app.Run();

static class EmployeesRepository
{
    private static List<Employee> employees = new List<Employee>
    {
        new Employee(1, "John Doe", "Engineer", 60000),
        new Employee(2, "Jane Smith", "Manager", 75000),
        new Employee(3, "Sam Brown", "Technician", 50000)
    };

    public static List<Employee> GetEmployees() => employees;
}

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Position { get; set; }
    public double Salary { get; set; }

    public Employee(int id, string name, string position, double salary)
    {
        Id = id;
        Name = name;
        Position = position;
        Salary = salary;
    }
}