using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace WebApp.Controllers
{
    public class FileResultsController : Controller
    {
        public string Index()
        {
            return $"This controller is {nameof(FileResultsController)}";
        }

        [Route("/download_vf")]
        public IActionResult ReturnVirtualFile()
        {
            // The file has to exist in the wwwroot folder
            return File("/readme.txt", "text/plain");
        }

        [Route("/download_pf")]
        public IActionResult ReturnPhysicalFile()
        {
            //HostingEnvironment.MapPath("~/Data/data.html");
            //this.Host.ResolvePath("readme.txt");
            // The file has to exist on the server
            return PhysicalFile(@"c:\temp\sample.pdf", "application/pdf");
        }

        [Route("/download_cf")]
        public IActionResult ReturnContentFile()
        {
            // This is a file that is generated in memory
            byte[] bytes = System.IO.File.ReadAllBytes(@"c:\temp\sample.pdf");

            return File(bytes, "application/pdf");
        }
    }
}
