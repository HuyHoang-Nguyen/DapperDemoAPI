namespace DapperDemoAPI.QueryModels
{
    public class GetPayrollReportModel
    {
        public int TotalEmployees { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal AverageSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public decimal MinSalary { get; set; }
    }
}
