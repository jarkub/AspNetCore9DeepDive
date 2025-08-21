using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    //[AutoValidateAntiforgeryToken]
    //[ValidateAntiForgeryToken]
    [IgnoreAntiforgeryToken(Order = 2000)]
    public class EmployeesModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; }

        public string? Message { get; private set; }

        public void OnGet()
        {
            Message = $"OnGet Id = {Id}";
            Console.WriteLine(Message);
        }

        public void OnGetFirst()
        {
            Message = $"OnGetFirst Id = {Id}";
            Console.WriteLine(Message);
        }

        public void OnGetSecond()
        {
            Message = $"OnGetSecond Id = {Id}";
            Console.WriteLine(Message);
        }
        //[ValidateAntiForgeryToken]
        public void OnPostSave()
        {
            Message = $"OnPostSave Id = {Id}";
            Console.WriteLine(Message);
        }
        //[ValidateAntiForgeryToken]
        public void OnPostDelete()
        {
            Message = $"OnPostDelete Id = {Id}";
            Console.WriteLine(Message);
        }


    }
}
