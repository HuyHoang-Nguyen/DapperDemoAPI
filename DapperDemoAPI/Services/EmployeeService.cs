using DapperDemoAPI.Entities;
using DapperDemoAPI.Enums.EnumError;
using DapperDemoAPI.IRepositories;
using DapperDemoAPI.Models.Employee;
using DapperDemoAPI.QueryModels;
using DapperDemoAPI.Services.Interfaces;
using DapperDemoAPI.Validators.Employees;

namespace DapperDemoAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }
        public async Task<int> CreateAsync(EmployeeModel emp)
        {
            var errors = new List<EnumEmployeeValidationError>();
            errors.AddRange(EmployeeValidator.ValidateCreate(emp));

            if (!string.IsNullOrEmpty(emp.Email) && await _employeeRepository.EmailExistAsync(emp.Email))
            {
                errors.Add(EnumEmployeeValidationError.EmailExisted);
            }
            if (emp.DepartmentId.HasValue)
            {
                var exists = await _departmentRepository.ExistAsync(emp.DepartmentId.Value);

                if (!exists)
                {
                    errors.Add(EnumEmployeeValidationError.DepartmentNotExist);
                }
            }
            if (errors.Any())
            {
                throw new Exception(string.Join(", ", errors));
            }
            return await _employeeRepository.CreateAsync(emp);
        }
        public async Task<IEnumerable<GetTopSalaryQueryModel>> GetTopAsync(int topN)
        {
            return await _employeeRepository.GetTopAsync(topN);
        }
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _employeeRepository.GetAllAsync();
        }
        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _employeeRepository.GetByIdAsync(id);
        }
        public async Task<int> UpdateAsync(int id, UpdateEmployeeModel emp)
        {
            var errors = EmployeeValidator.ValidateUpdate(emp);

            var exists = await _employeeRepository.GetByIdAsync(id);
            if (exists == null)
            {
                errors.Add(EnumEmployeeValidationError.EmployeeNotExisted);
            }
            if (errors.Any())
            {
                throw new Exception(string.Join(", ", errors));
            }
            return await _employeeRepository.UpdateAsync(id, emp);
        }
        public async Task<int> DeleteAsync(int id)
        {
            var exists = await _employeeRepository.GetByIdAsync(id);
            if (exists == null)
            {
                throw new Exception(EnumEmployeeValidationError.EmployeeNotExisted.ToString());
            }
            return await _employeeRepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<NewHireQueryModel>> GetNewHireMonthAsync(int year)
        {
            return await _employeeRepository.GetNewHireMonthAsync(year);
        }
    }
}
