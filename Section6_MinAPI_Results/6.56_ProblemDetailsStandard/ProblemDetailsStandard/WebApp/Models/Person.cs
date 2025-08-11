namespace WebApp.Models;

class Person
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public static ValueTask<Person?> BindAsync(HttpContext context)
    {
        var idStr = context.Request.Query["id"];
        var nameStr = context.Request.Headers["name"];
        if (string.IsNullOrWhiteSpace(nameStr))
        {
            nameStr = context.Request.Query["name"];
        }

        if (int.TryParse(idStr, out var id))
        {
            return new ValueTask<Person?>(new Person { Id = id, Name = nameStr });
        }

        return new ValueTask<Person?>(Task.FromResult<Person?>(null));
    }
}