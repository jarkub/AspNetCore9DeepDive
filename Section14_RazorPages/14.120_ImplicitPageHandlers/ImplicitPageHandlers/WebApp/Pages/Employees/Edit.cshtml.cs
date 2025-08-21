using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class EmployeesModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; }        

        public void OnGet()
        {
        }        

        public void OnPostSave()
        {

        }

        public void OnPostDelete()
        {

        }


    }
}
