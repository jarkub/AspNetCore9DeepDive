using Microsoft.AspNetCore.Mvc;

namespace WebApp.Pages.Shared.Components.MyViewComponent
{
    [ViewComponent( Name = "MyViewComponent")]
    public class MyViewComponent : ViewComponent
    {
        
        public IViewComponentResult Invoke(string message)
        {
            ViewData["message"] = $"My message: {message}";
            //var viewModel = new MyViewComponentModel { Message = message };
            var viewModel = new Model { Message = message };

            return View(viewModel);
            //return View(message);
        }

        public class MyViewComponentModel { 
            public string? Message { get; set; }
        }
    }

    public class Model
    {
        public string? Message { get; set; }
    }
}
