using System.ComponentModel.DataAnnotations;

namespace DapperDemoAPI.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string? FullName { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [MaxLength(12)]
        public string? Phone {  get; set; }
        public DateOnly HireDate { get; set; }

        [Required, MaxLength(20)]
        public int DepartmentId { get; set; }

        [Required]
        public decimal BaseSalary { get; set; }

        [Required, MaxLength(50)]
        public string? Status { get; set; }
    }
}
