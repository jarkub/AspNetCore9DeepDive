using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseRouting();

app.MapGet("/people", (Person? p) =>
{
    return $"Id is {p?.Id}; Name is {p?.Name}";
});

app.Run();

