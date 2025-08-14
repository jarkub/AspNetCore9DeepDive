using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace WebApp.Controllers
{
    public class FileResultsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileResultsController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public string Index()
        {
            return $"This controller is {nameof(FileResultsController)}";
        }

        [Route("/download_vf")]
        public IActionResult ReturnVirtualFile()
        {
            // The file has to exist in the wwwroot folder
            return File("/readme.txt", "text/plain"); //ControllerBase.File()
        }

        [Route("/download_pf")]
        public IActionResult ReturnPhysicalFile()
        {
            string defaultFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Program.txt");
            string wwwrootPath = _webHostEnvironment.WebRootPath;
            string[] filesInWwwroot = Directory.GetFiles(wwwrootPath);
            //string absoluteFilePath = filesInWwwroot.FirstOrDefault(f => f.EndsWith("readme.txt", StringComparison.OrdinalIgnoreCase)) ?? defaultFilePath;
            string absoluteFilePath = filesInWwwroot.FirstOrDefault() ?? defaultFilePath;
            // The file has to exist on the server
            return PhysicalFile(absoluteFilePath, "text/plain"); //ControllerBase.PhysicalFile()
        }

        [Route("/download_cf")]
        public IActionResult ReturnContentFile()
        {
            string defaultFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "ImportUsing.tt");
            // This is a file that is generated in memory
            byte[] bytes = System.IO.File.ReadAllBytes(defaultFilePath);

            return File(bytes, "text/plain");
        }

        [Route("/download_file")]
        public IActionResult DownloadFile()
        {
            // The others simplyy display the file contents in the browser because the mime type was text/plain,
            // which is both specific and a type the browser knows how to render.
            // This one will prompt the user to download the file because the mime type is application/octet-stream.
            // because the type is generic and so the browser won't know how to handle it by itself
            return File("/readme.txt", "application/octet-stream"); //ControllerBase.File()
        }

        [Route("/download_file/file.txt")]
        public IActionResult DownloadTextFile()
        {
            // The browser will name the download according to the last part of the path.
            // Here it will be named file.txt, which your operating system likely knows what to do with
            return File("/readme.txt", "application/octet-stream"); //ControllerBase.File()
        }
    }
}
