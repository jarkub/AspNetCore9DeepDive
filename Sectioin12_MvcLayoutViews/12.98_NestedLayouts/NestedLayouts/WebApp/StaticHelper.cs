namespace WebApp
{
    public static class StaticHelper
    {
        public static string GetStaticFilePath(string fileName)
        {
            return System.IO.Path.Combine("wwwroot", "static", fileName);
        }
        public static string GetStaticFileUrl(string fileName)
        {
            return $"/static/{fileName}";
        }

        public static string GetStaticFileUrlWithVersion(string fileName, string version)
        {
            return $"/static/{fileName}?v={version}";
        }

        public static string GetStaticFileUrlWithVersion(string fileName, int version)
        {
            return $"/static/{fileName}?v={version}";
        }

        public static string SayHello()
        {
            Console.WriteLine("Hello from StaticHelper!");
            return "Hello, World!";
        }

        public static string SayGoodbye()
        {
            Console.WriteLine("Goodbye from StaticHelper!");
            return "Goodbye, World!";
        }
    }
}