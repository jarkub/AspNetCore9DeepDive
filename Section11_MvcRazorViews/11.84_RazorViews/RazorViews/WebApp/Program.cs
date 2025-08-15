var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

//app.MapControllers(); // only supports attribute routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );


//app.UseRouting();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Departments}/{action=Index}/{id?}"
//        );
//});

app.Run();

public static class RenderCount
{
    private static int count = 0;
    public static int Increment()
    {
        count++;
        return count;
    }
}

public static class Document
{
    private static readonly string body =
"""
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <title>Turd Ferguson</title>
</head>
<body>
    {0}
</body>
</html>
""";

    public static string Render(string content)
    {
        return string.Format(body, content);
    }
}