using System.ComponentModel.DataAnnotations;

namespace DapperDemoAPI.Models.Employee
{
    public class EmployeeModel
    {
        [Required, MaxLength(200)]
        public string? FullName { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required, MaxLength(12)]
        public string? Phone { get; set; } 
        public DateOnly HireDate { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public decimal BaseSalary { get; set; }
    }
}
