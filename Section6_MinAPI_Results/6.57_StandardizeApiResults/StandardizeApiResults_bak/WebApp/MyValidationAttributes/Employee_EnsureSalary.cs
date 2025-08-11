using System.ComponentModel.DataAnnotations;
using WebApp.Models;
namespace WebApp.MyValidationAttributes;

public class Employee_EnsureSalary: ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var employee = validationContext.ObjectInstance as Employee;
        if (employee is null)
        {
            return new ValidationResult("Invalid employee object.");
        }
        if (value is not double salary)
        {
            return new ValidationResult("Invalid salary value.");
        }
        if (salary < 0)
        {
            return new ValidationResult("Salary cannot be negative.");
        }

        double minSalary = 0;
        string position = string.IsNullOrWhiteSpace(employee.Position) ? "Unknown" : employee.Position;
        if (position.Equals("Manager", StringComparison.OrdinalIgnoreCase))
        {
            minSalary = 100000;
            //return new ValidationResult("Managers must have a salary of at least $100,000.");
        }
        else if (position.Equals("Executive", StringComparison.OrdinalIgnoreCase))
        {
            minSalary = 150000;
        }

        if (salary < minSalary)
        {
            return new ValidationResult($"Salary must be at least ${minSalary} for the position of {position}.");
        }

        // Value already set by the framework
        //employee.Salary = salary;

        return ValidationResult.Success;
    }
}