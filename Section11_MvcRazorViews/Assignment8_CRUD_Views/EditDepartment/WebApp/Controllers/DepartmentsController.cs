using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebApp.Controllers
{
    public class DepartmentsController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var departments = DepartmentsRepository.GetDepartments();

            return View(departments);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var department = DepartmentsRepository.GetDepartmentById(id);
            if (department == null)
            {
                return View("Error", new List<string>() { "Department not found." });
            }

            return View(department);
            
        }

        [HttpPost]
        public IActionResult Edit(Department department)
        {
            if (!ModelState.IsValid)
            {
                return View("Error", GetErrors());
            }
            
            DepartmentsRepository.UpdateDepartment(department);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {           
            return View(new Department());
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (!ModelState.IsValid)
            {
                return View("Error", GetErrors());
            }

            DepartmentsRepository.AddDepartment(department);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var department = DepartmentsRepository.GetDepartmentById(id);
            if (department == null)
            {
                ModelState.AddModelError("id", "Department not found.");

                return View("Error", GetErrors());
            }

            DepartmentsRepository.DeleteDepartment(department);

            return RedirectToAction(nameof(Index));
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
}
