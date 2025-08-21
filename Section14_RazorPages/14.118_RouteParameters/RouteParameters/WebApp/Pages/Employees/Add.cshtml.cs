using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Employees
{
    public class AddModel : PageModel
    {
        public int? Id { get; set; }
        public void OnGet(int? id)
        {
            Id = id;
        }
    }
}
