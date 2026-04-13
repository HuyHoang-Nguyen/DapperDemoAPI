using DapperDemoAPI.QueryModels;

namespace DapperDemoAPI.Services.Interfaces
{
    public interface IPayrollService
    {
        Task<int> InsertPayrollAsync(int month, int year);

        Task<GetPayrollReportModel?> GetPayrollReportAsync(int month, int year);
    }
}
