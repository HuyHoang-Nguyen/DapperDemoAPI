using Dapper;
using DapperDemoAPI.Entities;
using DapperDemoAPI.IRepositories;
using Microsoft.Data.SqlClient;

namespace DapperDemoAPI.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly string _connectionString;
        public DepartmentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection missing");
        }
        public async Task<bool> ExistAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
                        var sql = "select case when exists " +
                "            (select 1 from Departments where Id = @Id and IsDeleted = 0) " +
                "            then cast (1 as bit) else cast (0 as bit) " +
                "            end";
            return await connection.ExecuteScalarAsync<bool>(sql, new { Id = id }); 
        }
        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Department>("select * from Departments where IsDeleted = 0");
        }
        public async Task<Department?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Department>("select * from Departments where Id = @Id and IsDeleted = 0", new { Id = id });
        }
        public async Task<int> CreateAsync(Department dpm)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"insert into Departments(Name) " +
                      "output inserted.Id " +
                      "values (@Name)";
            return await connection.QuerySingleAsync<int>(sql, dpm);  
        }
        public async Task<int> UpdateAsync(Department dpm)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"update Departments " +
                      "set Name = @Name " +
                      "where Id = @Id and IsDeleted = 0; ";
            return await connection.ExecuteAsync(sql, dpm);
        }
        public async Task<int> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"update Departments set IsDeleted = 1 where Id = @Id and IsDeleted = 0; ";
            return await connection.ExecuteAsync(sql, new {Id=id});
        }
        public async Task<IEnumerable<Department?>> GetEmptyDepartmentAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = ("select d.Name " +
                "from Departments d " +
                "left join Employees e on e.DepartmentId = d.Id " +
                "where e.DepartmentId is null and IsDeleted = 0; ");
            return await connection.QueryAsync<Department>(sql);
        }
    }
}
    