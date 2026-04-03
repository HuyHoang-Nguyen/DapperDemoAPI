namespace DapperDemoAPI.Entities
{
    public class Payroll
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal Bonus { get; set; }
        public decimal NetSalary { get; set; }
    }
}
