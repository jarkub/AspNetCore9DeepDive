using System.ComponentModel.DataAnnotations;
using WebApp.Models;
namespace WebApp.MyValidationAttributes;

public class Registration_EnsurePassword : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var registration = validationContext.ObjectInstance as Registration;
        if (registration is null)
        {
            return new ValidationResult("Invalid employee object.");
        }

        if (string.IsNullOrEmpty(registration.Password) || string.IsNullOrEmpty(registration.ConfirmPassword))
        {
            return new ValidationResult("Password and Confirm Password cannot be empty.");
        }

        if (registration.Password != registration.ConfirmPassword)
        {
            return new ValidationResult("Password and Confirm Password do not match.");
        }

        return ValidationResult.Success;
    }
}