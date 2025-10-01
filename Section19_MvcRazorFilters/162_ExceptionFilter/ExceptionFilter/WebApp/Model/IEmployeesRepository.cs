
namespace WebApp.Models
{
    public interface IEmployeesRepository
    {
        void AddEmployee(Employee? employee);
        bool DeleteEmployee(Employee? employee);
        Employee? GetEmployeeById(int id);
        List<Employee> GetEmployees(string? filter = null, int? departmentId = null);
        bool UpdateEmployee(Employee? employee);
    }
}