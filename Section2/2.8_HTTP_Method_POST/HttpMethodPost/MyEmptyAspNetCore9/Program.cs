
using Microsoft.Extensions.Primitives;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
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
    else if (context.Request.Method == "POST")
    {
        if (context.Request.Path.StartsWithSegments("/myclass"))
        {
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, // Enable case-insensitive matching
                //NumberHandling = JsonNumberHandling.AllowReadingFromString,
                //IncludeFields = true
            };
            MyClass obj = JsonSerializer.Deserialize<MyClass>(body, options);
            Console.WriteLine(obj);
        }
        if (context.Request.Path.StartsWithSegments("/employees"))
        {
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, // Enable case-insensitive matching
                //NumberHandling  = JsonNumberHandling.AllowReadingFromString,
                //IncludeFields = true
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver
                {
                    Modifiers = { SetNumberHandlingModifier }
                },
                Converters =
                {
                    new EmployeeConverter(),
                    //new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }


            };
            Employee? employeeJson = null;
            try
            {
                employeeJson = JsonSerializer.Deserialize<Employee>(body, options);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            //EmployeesRepository.AddEmployee(employeeJson);

            var formData = await context.Request.ReadFormAsync();
            var name = formData["name"].ToString();
            //var position = formData["position"].ToString();
            //var salary = double.Parse(formData["salary"]);
            //var newId = EmployeesRepository.GetEmployees().Count + 1;
            //var newEmployee = new Employee(newId, name, position, salary);
            //EmployeesRepository.GetEmployees().Add(newEmployee);
            //var employee = EmployeesRepository.GetEmployees().FirstOrDefault(e => e.Name == name);
            //context.Response.StatusCode = 201; // Created
            //await context.Response.WriteAsync($"New employee created: Id: {employee.Id}, Name: {employee.Name}, Position: {employee.Position}, Salary: {employee.Salary}");
            await context.Response.WriteAsync($"New employee created: {name}");
        }
    }
    else
    {
        context.Response.StatusCode = 404; // Not Found
        await context.Response.WriteAsync("Not Found");
    }
});

app.Run();

static void SetNumberHandlingModifier(JsonTypeInfo jsonTypeInfo)
{
    if (jsonTypeInfo.Type == typeof(int))
    {
        jsonTypeInfo.NumberHandling = JsonNumberHandling.AllowReadingFromString;
    }
    if (jsonTypeInfo.Type == typeof(double))
    {
        jsonTypeInfo.NumberHandling = JsonNumberHandling.AllowReadingFromString;
    }
}

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
            if (employee.Id <= 0)
            {
                employee = new Employee(
                    employees.Count + 1, 
                    employee.Name, 
                    employee.Position, 
                    employee.Salary);
                //employee.Id = employees.Count + 1; // Assign a new ID if not set
            }
            employees.Add(employee);
        }
    }
}

public class MyClass
{
    public int Id { get; }
    public string Name { get; }

    //[JsonConstructor]
    public MyClass(int id, string name)
    {
        Id = id;
        Name = name;
    }

    //[JsonConstructor]
    public MyClass(string name)
    {
        Id = 2;
        Name = name;
    }
}

public class EmployeeConverter : JsonConverter<Employee>
{
    public override Employee Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Manual parsing and constructor call
        // ... read properties from reader ...
        //propertyName, propertyValue
        return new Employee(1, "a", "b", 1);
    }

    public override void Write(Utf8JsonWriter writer, Employee value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    // ... other methods ...
}

public class Employee
{
    //[JsonPropertyName("id")]
    public int Id { get;/* private set;*/ }

    //[JsonPropertyName("name")]
    public string Name { get; /*private set;*/ }
    //[JsonPropertyName("position")]
    public string Position { get; /*private set;*/ }
    //[JsonPropertyName("salary")]
    public double Salary { get; /*private set;*/ }

    public Employee(int id, string name, string position, double salary)
    {
        Id = id;
        Name = name;
        Position = position;
        Salary = salary;
    }

    [JsonConstructor]
    public Employee(string name, string position, string salary)// : this(-1, name, position, -1)
    {
        Id = EmployeesRepository.GetEmployees().Count;

        double num = -1;
        double.TryParse(salary, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out num);

        Salary = num;

        Name = "";
        Position = "";
    }
}

//public class EmployeeChild : Employee
//{
//    public new string Salary
//    {
//        get; set;
//        //set
//        //{
//        //    double num;
//        //    if (double.TryParse(value, NumberStyles.AllowThousands,
//        //        CultureInfo.InvariantCulture, out num))
//        //    {
//        //        base.Salary = num;
//        //    }
//        //}
//    }

//    public EmployeeChild(int id, string name, string position, string salary)
//    {
//        double num = -1;
//        double.TryParse(salary, NumberStyles.AllowThousands,
//            CultureInfo.InvariantCulture, out num);

//        base(id, name, position, num);
//    }
//}