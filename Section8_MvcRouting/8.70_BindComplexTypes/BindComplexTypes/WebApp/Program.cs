var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    //.AddXmlDataContractSerializerFormatters()
    .AddXmlSerializerFormatters()
    ;

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
        );
});

app.Run();
