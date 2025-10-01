using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Pages.Employees
{
    public class EditModel : PageModel
    {
        private readonly IEmployeesRepository employeesRepository;
        private readonly IDepartmentsRepository departmentsRepository;

        public EditModel(IEmployeesRepository employeesRepository, IDepartmentsRepository departmentsRepository)
        {
            this.employeesRepository = employeesRepository;
            this.departmentsRepository = departmentsRepository;
        }

        [BindProperty]
        public EmployeeViewModel? EmployeeViewModel { get; set; }

        public void OnGet(int id)
        {
            this.EmployeeViewModel = new EmployeeViewModel();
            this.EmployeeViewModel.Employee = employeesRepository.GetEmployeeById(id);
            this.EmployeeViewModel.Departments = departmentsRepository.GetDepartments();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelStateHelper.GetErrors(ModelState);
                return RedirectToPage("/Error", new { errors });
            }

            if (EmployeeViewModel is not null && EmployeeViewModel.Employee != null)
            {
                employeesRepository.UpdateEmployee(EmployeeViewModel.Employee);
            }

            return RedirectToPage("Index");
        }

        public IActionResult OnPostDeleteEmplopyee(int id)
        {
            var employee = employeesRepository.GetEmployeeById(id);
            if (employee == null)
            {
                ModelState.AddModelError("id", "Employee not found");

                var errors = ModelStateHelper.GetErrors(ModelState);
                return RedirectToPage("/Error", new { errors });
            }

            employeesRepository.DeleteEmployee(employee);

            return RedirectToPage("Index");
        }

    }
}
