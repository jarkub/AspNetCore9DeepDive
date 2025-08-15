using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace WebApp.Controllers
{
    public class ContentResultsController : Controller
    {
        public string Index()
        {
            return $"This controller is {nameof(ContentResultsController)}";
        }

        public IActionResult TextPlain()
        {
            // IActionResult can't return string directly, must be obj that implements IActionResult
                var mediaTypeHeaderValue = Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse(MediaTypeNames.Text.Plain);
                mediaTypeHeaderValue.Encoding = System.Text.Encoding.UTF8;

                var contentResult = new ContentResult
            {
                Content = "Hello World!",
                ContentType = mediaTypeHeaderValue.ToString(),
                StatusCode = (int)HttpStatusCode.OK
            };

            // new {} vs new (){} ?
            var contentResult2 = new ContentResult()
            {
                Content = "Hello World!",
                ContentType = mediaTypeHeaderValue.ToString(),
                StatusCode = (int)HttpStatusCode.OK
            };
            bool match0 = contentResult == contentResult2;
            // Alternatively, you can use the Content helper method from ControllerBase. For some reason it doesn't accept StatusCode
            var content = Content("Hello World!", MediaTypeNames.Text.Plain, System.Text.Encoding.UTF8);
            content.StatusCode = (int)HttpStatusCode.OK;

            bool match1 = contentResult.Equals(content);
            bool match2 = contentResult == content;

            return contentResult;
        }

        public IActionResult TextHtml()
        {
            // IActionResult can't return string directly, must be obj that implements IActionResult

            var contentResult = new ContentResult
            {
                Content = "<h1>Hello World!</h1>",
                ContentType = MediaTypeNames.Text.Html,
                StatusCode = (int)HttpStatusCode.OK
            };

            return contentResult;
        }
    }
}
