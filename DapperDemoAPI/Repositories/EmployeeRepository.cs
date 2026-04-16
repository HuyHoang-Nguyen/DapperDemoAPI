using Dapper;
using DapperDemoAPI.Entities;
using DapperDemoAPI.IRepositories;
using DapperDemoAPI.Models;
using DapperDemoAPI.Models.Employee;
using DapperDemoAPI.QueryModels;
using System.Data;
using System.Diagnostics.Eventing.Reader;


namespace DapperDemoAPI.Repositories
{
    public class EmployeeRepository : BaseRepository, IEmployeeRepository
    {
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {   
        }
        public async Task<bool> EmailExistAsync(string email)
        {
            using var connection = CreateConnection();
            var sql = "select case when exists " +
                "            (select 1 from Employees where Email = @Email) " +
                "            then cast (1 as bit) else cast (0 as bit) " +
                "            end";
            return await connection.ExecuteScalarAsync<bool>(sql, new { Email = email });
        }
        public async Task<IEnumerable<GetTopSalaryQueryModel>> GetTopAsync(int topN)
        {
            using var connection = CreateConnection();
            var sql = @"select top (@N) 
                            Id, FullName, BaseSalary 
                        from Employees 
                        where IsDeleted = 0 
                        order by BaseSalary desc ";
            return await connection.QueryAsync<GetTopSalaryQueryModel>(sql, new { N = topN });
        }
        public async Task<int> CreateAsync(EmployeeModel emp)
        {
            using var connection = CreateConnection();
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
            using var connection = CreateConnection();
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
            using var connection = CreateConnection();
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
            using var connection = CreateConnection();
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
            using var connection = CreateConnection();
            var sql = "update Employees " +
                "      set IsDeleted = 1 " +
                "      where Id = @Id " +
                "      and IsDeleted = 0; ";
            return await connection.ExecuteAsync(sql, new {Id = id});
        }
        public async Task<IEnumerable<NewHireQueryModel>> GetNewHireMonthAsync(int year)
        {
            using var connection = CreateConnection();
            var query = await connection.QueryAsync<NewHireQueryModel>("sp_Report_NewEmployeesByMonth", new { year = year }, commandType: CommandType.StoredProcedure);
            return query.ToList();
        }

        public async Task<PagingResult<Employee>> SearchAsync(string? keyword, int? departmentId, decimal? minSalary, decimal? maxSalary, string? Status, int page, int pageSize, string sortBy, string sortDir)
        {
            using var connection = CreateConnection();
            var parameters = new
            {
                keyword = keyword,
                departmentId = departmentId,
                minSalary = minSalary,
                maxSalary = maxSalary,
                Status = Status,
            };
            var result = await connection.QueryAsync<GetEmployeeQuery>("sp_SearchEmployees", parameters, commandType: CommandType.StoredProcedure);
            var pagedResult = result.Select(e => new Employee
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
            var ascending = sortDir.ToLower() != "desc";
            if (sortBy == "fullname")
            {
                if (ascending)
                {
                    pagedResult = pagedResult.OrderBy(e => e.FullName);
                }
                else
                {
                    pagedResult = pagedResult.OrderByDescending(e => e.FullName);
                }
            }
            else if (sortBy == "email")
            {
                if (ascending)
                {
                    pagedResult = pagedResult.OrderBy(e => e.Email);
                }
                else
                {
                    pagedResult = pagedResult.OrderByDescending(e => e.Email);
                }
            }
            else if (sortBy == "phone")
            {
                if (ascending)
                {
                    pagedResult = pagedResult.OrderBy(e => e.Phone);
                }
                else
                {
                    pagedResult = pagedResult.OrderByDescending(e => e.Phone);
                }
            }
            else if (sortBy == "hiredate")
            {
                if (ascending)
                {
                    pagedResult = pagedResult.OrderBy(e => e.HireDate);
                }
                else
                {
                    pagedResult = pagedResult.OrderByDescending(e => e.HireDate);
                }
            }
            else if (sortBy == "departmentid")
            {
                if (ascending)
                {
                    pagedResult = pagedResult.OrderBy(e => e.DepartmentId);
                }
                else
                {
                    pagedResult = pagedResult.OrderByDescending(e => e.DepartmentId);
                }
            }
            else if (sortBy == "basesalary")
            {
                if (ascending)
                {
                    pagedResult = pagedResult.OrderBy(e => e.BaseSalary);
                }
                else
                {
                    pagedResult = pagedResult.OrderByDescending(e => e.BaseSalary);
                }
            }
            else if (sortBy == "status")
            {
                if (ascending)
                {
                    pagedResult = pagedResult.OrderBy(e => e.Status);
                }
                else
                {
                    pagedResult = pagedResult.OrderByDescending(e => e.Status);
                }
            }
            else
            {
                if (ascending)
                {
                    pagedResult = pagedResult.OrderBy(e => e.Id);
                }
                else
                {
                    pagedResult = pagedResult.OrderByDescending(e => e.Id);
                }
            }
            var totalPage = pagedResult.Count();
            var data = pagedResult.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return new PagingResult<Employee>
            {
                Data = data,
                TotalPages = totalPage,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
