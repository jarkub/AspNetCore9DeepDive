using System.ComponentModel.DataAnnotations;
using WebApp.MyValidationAttributes;

namespace WebApp.Models;

public class Employee
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Position { get; set; }

    //[Required]
    //[Range(50000, 200000)]
    [Employee_EnsureSalary] // Custom validation attribute
    public double Salary { 
        get; 
        set; 
    }

    public Employee(int id, string name, string position, double salary)
    {
        Id = id;
        Name = name;
        Position = position;
        Salary = salary;
    }
}
