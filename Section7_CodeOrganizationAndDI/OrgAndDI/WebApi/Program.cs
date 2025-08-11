using System.Text.Json;
using WebApi.Endpoints;
using WebApi.Models;
using WebApi.Results;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

app.UseStatusCodePages();

app.MapEmployeeEndpoints();

app.Run();
