using Microsoft.AspNetCore.Mvc;

using WebApp.Models;

namespace ViewComponents.Views.Departments.Components.DepartmentList
{
    [ViewComponent]
    public class DepartmentList : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var departments = DepartmentsRepository.GetDepartments();
            return View(departments);
        }


    }
}
