using Microsoft.Extensions.Primitives;
using System.Text;
using System.Web;

var builder = WebApplication.CreateBuilder(args); // Creates Kestrel web server
var app = builder.Build(); // Creates web app

//app.MapGet("/", () => "Hello World!"); // middleware to handle GET requests at the root URL

// This middleware will handle all requests
app.Run(async (HttpContext context) =>
{
    //await context.Response.WriteAsync(context.Request.QueryString.ToString(), Encoding.UTF8);

    foreach (var key in context.Request.Query.Keys)
    {
        await context.Response.WriteAsync($"{key}: {context.Request.Query[key]}{Environment.NewLine}");
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

    public static void AddEmployee(Employee? employee)
    {
        if (employee is not null)
        {
            employees.Add(employee);
        }
    }

    public static bool UpdateEmployee(Employee? employee)
    {
        if (employee is not null)
        {
            var emp = employees.FirstOrDefault(x => x.Id == employee.Id);
            if (emp is not null)
            {
                emp.Name = employee.Name;
                emp.Position = employee.Position;
                emp.Salary = employee.Salary;

                return true;
            }
        }

        return false;
    }
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