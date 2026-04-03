using DapperDemoAPI.Entities;
using DapperDemoAPI.Models.Employee;
using DapperDemoAPI.QueryModels;
using System.Threading.Tasks;

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
    }
}
