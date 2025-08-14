
namespace WebApi;

public static class MyConsole
{
    public static void WriteObject(object? obj)
    {
        Console.WriteLine("{0} | {1} | {2}", nameof(obj), obj?.GetType().FullName, obj);
    }
}