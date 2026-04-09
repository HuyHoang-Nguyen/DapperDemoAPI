using DapperDemoAPI.Common.Results;
using DapperDemoAPI.Entities;
using DapperDemoAPI.Models.Employee;
using DapperDemoAPI.QueryModels;

namespace DapperDemoAPI.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<GetTopSalaryQueryModel>> GetTopAsync(int topN);
        Task<int> CreateAsync(EmployeeModel emp);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<MethodResult<int>> UpdateAsync(int id, UpdateEmployeeModel emp);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<NewHireQueryModel>> GetNewHireMonthAsync(int year);
    }
}

