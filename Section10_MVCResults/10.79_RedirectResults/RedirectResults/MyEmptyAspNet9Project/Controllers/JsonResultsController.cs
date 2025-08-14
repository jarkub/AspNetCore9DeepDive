using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
//using System.Web.Http;

using WebApi.Models;

namespace WebApp.Controllers
{
    public class JsonResultsController : Controller
    {
        public string Index()
        {
            return $"This controller is {nameof(JsonResultsController)}";
        }

        public object GetDepartmentById(int id)
        {
            // this will default to Json but if the client's Accept header is "text/xml" it will return XML
            Console.WriteLine($"{nameof(GetDepartmentById)}, Accept: {HttpContext.Request.Headers.Accept}");
            return new Department
            {
                Id = id,
                Name = "Department " + id,
                Description = "Description for department " + id
            };

        }

        public object GetAnonymousObjectById(int id)
        {
            // this seems to always return Json although its type is "AnonymousType"
            //System.InvalidOperationException: <>f__AnonymousType0`3[System.Int32,System.String,System.String] Anonymous cannot be serialized because it does not have a parameterless constructor.
            var res = new
            {
                Id = id,
                Name = "Department " + id,
                Description = "Description for department " + id
            };
            Console.WriteLine($"{nameof(GetAnonymousObjectById)}, Accept: {HttpContext.Request.Headers.Accept}");
            return res;
        }

        public object GetJsonObjectById(int id)
        {
            // this will awlays return Json regardless of the client's Accept header
            var res = new
            {
                Id = id,
                Name = "Department " + id,
                Description = "Description for department " + id
            };

            var jr = new JsonResult(res)
            {
                StatusCode = (int)HttpStatusCode.OK,
                ContentType = MediaTypeNames.Application.Json
            };

            var j = Json(res);

            bool isMatch = jr.Equals(j);
            bool isTypeMatch = jr.GetType() == j.GetType();
            Console.WriteLine($"{nameof(GetJsonObjectById)}, Accept: {HttpContext.Request.Headers.Accept}");
            return Json(res);
        }

        public JsonResult GetJsonResultById(int id)
        {
            // this will awlays return Json regardless of the client's Accept header
            var res = new
            {
                Id = id,
                Name = "Department " + id,
                Description = "Description for department " + id
            };

            var jr = new JsonResult(res)
            {
                StatusCode = (int)HttpStatusCode.OK,
                ContentType = MediaTypeNames.Application.Json
            };

            var j = Json(res);

            bool isMatch = jr.Equals(j);
            bool isTypeMatch = jr.GetType() == j.GetType();
            Console.WriteLine($"{nameof(GetJsonResultById)}, Accept: {HttpContext.Request.Headers.Accept}");
            return Json(res);
        }

        //[AllowAnonymous]
        public IActionResult GetIActResJsonById(int id)
        {
            // JsonResult implements IActionResult so this will awlays return Json regardless of the client's Accept header
            var res = new
            {
                Id = id,
                Name = "Department " + id,
                Description = "Description for department " + id
            };

            Console.WriteLine($"{nameof(GetIActResJsonById)}, Accept: {HttpContext.Request.Headers.Accept}");
            return Json(res);
        }
    }
}
