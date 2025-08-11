using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Route("/api")]
    public class DepartmentsController
    {
        [HttpGet("departments")]        
        public string GetDepartments()
        {
            return "These are the departments.";
        }

        [HttpGet("departments/{id}")]        
        public string GetDepartmentById(int id)
        {
            return $"Department info: {id}";
        }

      
    }
}
