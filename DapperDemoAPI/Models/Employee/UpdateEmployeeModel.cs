using System.ComponentModel.DataAnnotations;

namespace DapperDemoAPI.Models.Employee
{
    public class UpdateEmployeeModel
    {
        [MaxLength(100)]
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public int? DepartmentId { get; set; }
        public decimal? BaseSalary { get; set; }
    }
}
