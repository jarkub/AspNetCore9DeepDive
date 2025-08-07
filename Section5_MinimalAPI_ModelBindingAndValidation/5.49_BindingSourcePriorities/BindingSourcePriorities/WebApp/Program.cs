using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseRouting();

app.MapGet("/people", (Person? p) =>
{
    return $"Id is {p?.Id}; Name is {p?.Name}";
});

app.MapGet("/bindroute/{id}", (int id) =>
{
    return $"Id is {id}";
});

app.MapGet("/bindquery", (int id) =>
{
    return $"Id is {id}";
});

app.MapGet("/any{thing}", (string thing) =>
{
    return $"thing is {thing}";
});

app.MapGet("/every{thing}/{id}", (string thing, int id) =>
{
    return $"thing is {thing}, id is {id}";
});

app.Run();

