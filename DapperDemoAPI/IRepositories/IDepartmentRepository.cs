using DapperDemoAPI.Entities;

namespace DapperDemoAPI.IRepositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department?> GetByIdAsync(int id);
        Task<int> CreateAsync(Department dpm);
        Task<int> UpdateAsync(Department dpm);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<Department?>> GetEmptyDepartmentAsync();
        Task<bool> ExistAsync(int id);
    }
}
