using Dapper;
using DapperDemoAPI.Entities;
using DapperDemoAPI.IRepositories;
using DapperDemoAPI.Models.Employee;
using DapperDemoAPI.QueryModels;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using System.Runtime.InteropServices;


namespace DapperDemoAPI.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;
        public EmployeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<bool> EmailExistAsync(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "select case when exists " +
                "            (select 1 from Employees where Email = @Email) " +
                "            then cast (1 as bit) else cast (0 as bit) " +
                "            end";
            return await connection.ExecuteScalarAsync<bool>(sql, new { Email = email });
        }
        public async Task<IEnumerable<GetTopSalaryQueryModel>> GetTopAsync(int topN)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"select top (@N) 
                            Id, FullName, BaseSalary 
                        from Employees 
                        where IsDeleted = 0 
                        order by BaseSalary desc ";
            return await connection.QueryAsync<GetTopSalaryQueryModel>(sql, new { N = topN });
        }
        public async Task<int> CreateAsync(EmployeeModel emp)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "insert into Employees(FullName, Email, Phone, HireDate, DepartmentId, BaseSalary) " +
                "      output inserted.Id " +
                "      values(@FullName, @Email, @Phone, @HireDate, @DepartmentId, @BaseSalary) ";

            var employee = new
            {
                emp.FullName,
                emp.Email,
                emp.Phone,
                HireDate = emp.HireDate.ToDateTime(TimeOnly.MinValue),
                emp.DepartmentId,
                emp.BaseSalary
            };
            return await connection.QuerySingleAsync<int>(sql, employee);
        }
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "select * from Employees where IsDeleted = 0 ";
            var employees = await connection.QueryAsync<GetEmployeeQuery>(sql);

            return employees.Select(e => new Employee
            {
                Id = e.Id,
                FullName = e.FullName,
                Email = e.Email,
                Phone = e.Phone,
                HireDate = DateOnly.FromDateTime(e.HireDate),
                DepartmentId = e.DepartmentId,
                BaseSalary = e.BaseSalary,
                Status = e.Status
            });
        }
        public async Task<Employee?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "select * from Employees where IsDeleted = 0 and Id = @Id ";
            var e = await connection.QueryFirstOrDefaultAsync<GetEmployeeQuery?>(sql, new { Id = id });

            if (e == null)
            {
                return null;
            }
            return new Employee
            {
                Id = e.Id,
                FullName = e.FullName,
                Email = e.Email,
                Phone = e.Phone,
                HireDate = DateOnly.FromDateTime(e.HireDate),
                DepartmentId = e.DepartmentId,
                BaseSalary = e.BaseSalary,
                Status = e.Status
            };
        }
        public async Task<int> UpdateAsync(int id, UpdateEmployeeModel emp)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "update Employees " +
                "      set FullName = coalesce(@FullName, FullName), " +
                "      Phone = coalesce(@Phone, Phone), " +
                "      DepartmentId = coalesce(@DepartmentId, DepartmentId), " +
                "      BaseSalary = coalesce(@BaseSalary, BaseSalary) " +
                "      where Id = @Id and IsDeleted = 0 ";
            return await connection.ExecuteAsync(sql, new
            {
                Id = id,
                emp.FullName,
                emp.Phone,
                emp.DepartmentId,
                emp.BaseSalary
            });
        }
        public async Task<int> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "update Employees " +
                "      set IsDeleted = 1 " +
                "      where Id = @Id ";
            return await connection.ExecuteAsync(sql, new {Id = id});
        }
        public async Task<IEnumerable<NewHireQueryModel>> GetNewHireMonthAsync(int year)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = await connection.QueryAsync<NewHireQueryModel>("sp_Report_NewEmployeesByMonth", new { year = year }, commandType: CommandType.StoredProcedure);
            return query.ToList();
        }
    }
}
