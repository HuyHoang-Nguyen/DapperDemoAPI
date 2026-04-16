namespace DapperDemoAPI.Models.Employee
{
    public class GetEmployeeQuery
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime HireDate { get; set; }
        public int DepartmentId { get; set; }
        public decimal BaseSalary { get; set; }
        public string? Status { get; set; }
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string sortBy { get; set; } = "Id";
        public string sortOrder { get; set; } = "asc";
    }
}
