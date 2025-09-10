using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

using AuthMvc.Policies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

// Program.cs
builder.Services.AddSingleton<IAuthorizationHandler, MinimumAgeAuthorizationHandler>();
builder.Services.AddScoped<IClaimsTransformation, CustomClaimsTransformation>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        
        options.LoginPath = "/Home/Login"; // Redirect for unauthenticated users
        options.AccessDeniedPath = "/Home/AccessDenied"; // Redirect for authenticated but unauthorized users
    });

builder.Services.AddAuthorization(options =>
{
    
    options.AddPolicy("AtLeast21", policy =>
        policy.Requirements.Add(new MinimumAgeRequirement(21)));

    options.AddPolicy("RequireAdminRole", policy =>
    {
        Console.WriteLine("Defining RequireAdminRole policy");
        policy.RequireRole("Admin");
    });
    options.AddPolicy("CanEditContent", policy =>
    {
        Console.WriteLine("Defining RequireAdminRole policy");
        policy.RequireClaim("Permission", "EditContent");
    });

});

//builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
