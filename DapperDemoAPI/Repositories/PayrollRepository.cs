using Dapper;
using DapperDemoAPI.Entities;
using DapperDemoAPI.IRepositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperDemoAPI.Repositories
{
    public class PayrollRepository : IPayrollRepository
    {
        private readonly string _connectionString;
        public PayrollRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<int> InsertPayrollAsync(int month, int year)
        {
            using var connection = new SqlConnection(_connectionString);
            var inserted = await connection.ExecuteAsync("sp_RunPayrollMonth", new { month = month, year = year }, commandType: CommandType.StoredProcedure);
            return inserted;
        }
    }
}
