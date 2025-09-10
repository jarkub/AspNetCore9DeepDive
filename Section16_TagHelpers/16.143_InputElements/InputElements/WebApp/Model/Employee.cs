using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace WebApp.Models
{
    public class Employee
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Position { get; set; }
        [Required]
        public double? Salary { get; set; }

        [DisplayName("Department")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Department")]
        public int DepartmentId { get; set; }

        public Department? Department { get; set; }

        public Employee()
        {            
        }

        public Employee(int id, string name, string position, double salary, int departmentId)
        {
            Id = id;
            Name = name;
            Position = position;
            Salary = salary;
            DepartmentId = departmentId;
        }
    }
}
