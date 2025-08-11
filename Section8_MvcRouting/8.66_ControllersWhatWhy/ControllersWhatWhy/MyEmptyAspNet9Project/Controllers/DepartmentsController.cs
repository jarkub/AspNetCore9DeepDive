using Microsoft.AspNetCore.Mvc;

namespace ControllersWhatWhy.Controllers;

public class DepartmentsController
{
    public string GetDepartments()
    {
        return "Departments List";
    }

    public string GetDepartmentById(int id)
    {
        return $"Department with ID: {id}";
    }

    [NonAction]
    public string GetDepartmentByName(string name)
    {
        return $"Department with Name: {name}";
    }
}
