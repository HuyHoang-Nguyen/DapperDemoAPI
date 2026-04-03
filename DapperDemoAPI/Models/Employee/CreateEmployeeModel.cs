namespace DapperDemoAPI.Models.Employee
{
    public class CreateEmployeeModel
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateOnly HireDate { get; set; }
        public int DepartmentId { get; set; }
        public decimal BaseSalary { get; set; }
    }
}
