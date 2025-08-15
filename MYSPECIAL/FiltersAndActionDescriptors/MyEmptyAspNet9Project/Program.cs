using Microsoft.Extensions.DependencyInjection.Extensions;

using System.Web.Http;
using System.Web.Http.Dispatcher;

using WebApi;
using WebApi.Models;
using WebApi.Filters;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration.Get<HttpConfiguration>();
config ??= new HttpConfiguration();

builder.Services.AddControllers(options =>
    {
        // Add an instance of your filter
        // Or, for filters resolved from DI:
        // options.Filters.Add<MyGlobalFilter>();)
        //options.Filters.Add(new MyGlobalActionFilter());
        //options.Filters.Add(new MyAuthorizationFilter());
        options.Filters.Add(new CustomLogActionFilter());

        options.Conventions.Add(new CustomControllerConvention(/*new Something(config)*/));
    }) 
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

app.Run();