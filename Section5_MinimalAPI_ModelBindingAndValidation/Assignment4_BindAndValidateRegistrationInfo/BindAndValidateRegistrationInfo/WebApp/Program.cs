
using Microsoft.AspNetCore.Mvc;

using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/registration", (Registration employee) =>
{
    return "Employee is added successfully.";
}).WithParameterValidation();

app.MapPost("/registration", ([FromBody]Registration employee) =>
{
    return "Employee is added successfully.";
}).WithParameterValidation();

/*
app.MapPost("/registration", ([FromQuery] string Email, [FromQuery] string Password, [FromQuery] string ConfirmPassword) =>
{
    var employee = new Registration
    {
        Email = Email,
        Password = Password,
        ConfirmPassword = ConfirmPassword
    };
    return "Employee is added successfully.";
}).WithParameterValidation();
*/

/*
app.MapPost("/registration", (string Email, string Password, string ConfirmPassword) =>
{
    var employee = new Registration
    {
        Email = Email,
        Password = Password,
        ConfirmPassword = ConfirmPassword
    };
    return "Employee is added successfully.";
}).WithParameterValidation();
*/

app.Run();






