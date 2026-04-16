using DapperDemoAPI.Entities;
using DapperDemoAPI.Models;
using DapperDemoAPI.Models.Employee;
using DapperDemoAPI.QueryModels;

namespace DapperDemoAPI.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<GetTopSalaryQueryModel>> GetTopAsync(int topN);
        Task<int> CreateAsync(EmployeeModel emp);
        Task<bool> EmailExistAsync(string email);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<int> UpdateAsync(int id, UpdateEmployeeModel emp);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<NewHireQueryModel>> GetNewHireMonthAsync(int year);
        Task<PagingResult<Employee>> SearchAsync(string? keyword, int? departmentId, decimal? minSalary, decimal? maxSalary, string? Status, int page, int pageSize, string sortBy, string sortDir);
    }
}
