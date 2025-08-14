

using MvcResultTypes.MyMiddleware;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseStaticWebAssets();

builder.Services.AddTransient<IgnoreFavicon>();

builder.Services.AddControllers() 
    .AddXmlSerializerFormatters()
    ;

//builder.Services.AddSingleton<IHttpControllerSelector, Something>();
//builder.Services.TryAddSingleton<IHttpControllerSelector>(new Something(config));


// Register the IHttpControllerSelector service with a custom implementation
//builder.Services.Replace(new ServiceDescriptor(
//    typeof(IHttpControllerSelector),
//    sp => new Something(config),
//    ServiceLifetime.Singleton
//));

var app = builder.Build();

app.UseMiddleware<IgnoreFavicon>();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
        );
    //endpoints.MapControllers();
});

//app.MapControllers();

app.UseStaticFiles(); // Enable serving static files from wwwroot

app.Run();