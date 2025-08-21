using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Pages
{
    public class EmployeesModel : PageModel
    {
        [BindProperty(SupportsGet = true)]        
        public int? Id { get; set; }

        [BindProperty]
        public InputModel? InputModel { get; set; }

        public IActionResult OnGet()
        {
            //if (!ModelState.IsValid)
            //{
            //    this.InputModel!.ErrorMessages = GetErrors();

            //    return Page();
            //}

            return Page();
        }

        public IActionResult OnPostSave()
        {
            if (!ModelState.IsValid)
            {
                this.InputModel!.ErrorMessages = GetErrors();

                //return Page();
            }

            // do something with the input model
            //return RedirectToPage("Index");
            return Page();
        }

        public IActionResult OnPostDelete()
        {
            // delete info

            //redirect
            return RedirectToPage("Index");
        }

        private List<string> GetErrors()
        {
            List<string> errorMessages = new List<string>();
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    errorMessages.Add(error.ErrorMessage);
                }
            }

            return errorMessages;
        }


    }

    public class InputModel
    {        
        public int? Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public List<String>? ErrorMessages { get; set; }
    }
}