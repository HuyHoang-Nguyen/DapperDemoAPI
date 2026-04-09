using Dapper;
using DapperDemoAPI.IRepositories;
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
    }
}
