using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Filters;
using WebApp.Helpers;
using WebApp.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebApp.Controllers
{
    [WriteToConsoleResourceFilter(Description = "Departments Controller")]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentsRepository departmentsRepository;

        public DepartmentsController(IDepartmentsRepository departmentsRepository)
        {
            this.departmentsRepository = departmentsRepository;
        }

        [HttpGet]
        [WriteToConsoleResourceFilter(Description = "Index Method", Order = -1)]
        public IActionResult Index()
        { 
            return View();
        }

        //[Route("/department-list/{filter?}")]
        //public IActionResult SearchDepartments(string? filter)
        //{   
        //    var departments = departmentsRepository.GetDepartments(filter);
        //    return PartialView("_DepartmentList", departments);
        //}

        [Route("/department-list/{filter?}")]
        public IActionResult SearchDepartments(string? filter)
        {
            return ViewComponent("DepartmentList", new { filter });
        }

        [HttpGet]
        [EndpointExpiresFilter(ExpiryDate = "2028-01-18")]
        public IActionResult Details(int id)
        {
            var department = departmentsRepository.GetDepartmentById(id);
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
                return View("Error", ModelStateHelper.GetErrors(ModelState));
            }
            
            departmentsRepository.UpdateDepartment(department);

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
                return View("Error", ModelStateHelper.GetErrors(ModelState));
            }

            departmentsRepository.AddDepartment(department);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var department = departmentsRepository.GetDepartmentById(id);
            if (department == null)
            {
                ModelState.AddModelError("id", "Department not found.");

                return View("Error", ModelStateHelper.GetErrors(ModelState));
            }

            departmentsRepository.DeleteDepartment(department);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [HandleExceptionsFilter]
        public IActionResult GetDepartments()
        {
            throw new ApplicationException("Testing exception handling for web api endpoints.");
            var departments = departmentsRepository.GetDepartments();
            return Json(departments);
        }
        
    }
}
