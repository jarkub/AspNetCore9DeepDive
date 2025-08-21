using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    [BindProperties(SupportsGet = true)]
    public class EmployeesModel : PageModel
    {
        //[BindProperty(SupportsGet = true)]
        public int? Id { get; set; }

        //[BindProperty]
        public InputModel? InputModel { get; set; }

        public string? Department { get; set; }

        public void OnGet()
        {
        }

        public void OnPostSave(int? id)
        {
            Console.WriteLine($"OnPostSave called with Id: {Id} and id: {id}");
            Console.WriteLine($"InputModel Id: {InputModel.Id} and Name: {InputModel.Name}");
        }

        public void OnPostDelete()
        {

        }


    }

    public class InputModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }

        public int DepartmentNumber { get; set; }
    }
}