using System.ComponentModel.DataAnnotations;
using System.Text.Json;

using WebApp.MyValidationAttributes;
namespace WebApp.Models;

public class Registration
{
    [Required]
    [EmailAddress (ErrorMessage = "Invalid email address format.")]
    public string? Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string? Password { get; set; }

    [Required]
    [Display(Name = "Confirm Password")]
    [Registration_EnsurePassword]
    public string? ConfirmPassword { get; set; }

    public static ValueTask<Registration?> BindAsync(HttpContext context)
    {
        var email = context.Request.Query["email"];
        var pw = context.Request.Query["Password"];
        var pw2 = context.Request.Query["ConfirmPassword"];

        //if (string.IsNullOrWhiteSpace(email))
        //{
        //    using var reader = new StreamReader(context.Request.Body);
        //    var body = await reader.ReadToEndAsync();
        //    var result = JsonSerializer.Deserialize<Registration>(body);
        //    return new ValueTask<Registration?>(result);
        //}


        try
        {
            return new ValueTask<Registration?>(new Registration { Email = email, Password = pw, ConfirmPassword = pw2 });
        }
        catch (Exception)
        {
            // Handle any exceptions that may occur during binding
            return new ValueTask<Registration?>(Task.FromResult<Registration?>(null));
        }
    }
}
