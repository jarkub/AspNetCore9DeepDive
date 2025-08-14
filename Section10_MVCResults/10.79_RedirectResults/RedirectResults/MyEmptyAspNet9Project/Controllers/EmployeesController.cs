using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace WebApp.Controllers
{
    public class EmployeesController : Controller
    {
        public string Index()
        {
            return $"This controller is {nameof(EmployeesController)}";
        }

        public IActionResult GetEmployeesByDepartment(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest("Invalid department ID."); // 400
            }
            // Simulate fetching employees from a data source
            var employees = new[]
            {
                new { Id = 1, Name = "Alice", DepartmentId = 1 },
                new { Id = 2, Name = "Bob", DepartmentId = 1 },
                new { Id = 3, Name = "Charlie", DepartmentId = 2 }
            };
            var departmentEmployees = System.Linq.Enumerable.Where(employees, e => e.DepartmentId == id);
            if (System.Linq.Enumerable.Any(departmentEmployees))
            {
                return Ok(new
                {
                    Message = $"Getting employees for department {id}",
                    Employees = departmentEmployees
                });
            }
            else
            {
                return NotFound($"No employees found for department ID {id}."); // 404
            }
        }
    }
}
