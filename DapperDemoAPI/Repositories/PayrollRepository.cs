using Dapper;
using DapperDemoAPI.IRepositories;
using DapperDemoAPI.QueryModels;
using System.Data;

namespace DapperDemoAPI.Repositories
{
    public class PayrollRepository : BaseRepository, IPayrollRepository
    {
        public PayrollRepository(IConfiguration configuration) :base(configuration) 
        {
        }
        public async Task<int> InsertPayrollAsync(int month, int year)
        {
            using var connection = CreateConnection();
            var inserted = await connection.ExecuteAsync("sp_RunPayrollMonth", new { month = month, year = year }, commandType: CommandType.StoredProcedure);
            return inserted;
        }
        public async Task<GetPayrollReportModel?> GetPayrollReportAsync(int month, int year)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<GetPayrollReportModel>("sp_Report_TotalPayroll", new {month = month, year = year}, commandType: CommandType.StoredProcedure);
        }
    }
}
