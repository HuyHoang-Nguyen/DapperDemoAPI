using DapperDemoAPI.Common.Results;
using DapperDemoAPI.Entities;
using DapperDemoAPI.Enums.EnumError;
using DapperDemoAPI.GlobalExceptionHandler;
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

        //MethodResult

        //public async Task<MethodResult<int>> CreateAsync(EmployeeModel emp)
        //{
        //    var result = new MethodResult<int>();

        //    var validateErrors = EmployeeValidator.ValidateCreate(emp);
        //    foreach (var error in validateErrors)
        //    {
        //        result.AddError(error.ToString());
        //    }
        //    if (!string.IsNullOrEmpty(emp.Email) && await _employeeRepository.EmailExistAsync(emp.Email))
        //    {
        //        result.AddError("EmailExisted", "Email", emp.Email);
        //    }
        //    var exists = await _departmentRepository.ExistAsync(emp.DepartmentId);
        //    if (!exists)
        //    {
        //    result.AddError("DepartmentNotExisted", "DepartmentId", emp.DepartmentId);
        //    }
        //    if (!result.IsOK)
        //    {
        //        return result;
        //    }
        //    var id = await _employeeRepository.CreateAsync(emp);

        //    result.Result = id;
        //    result.StatusCode = 201;
        //    return result;
        //}

        //Middleware Exception

        public async Task<int> CreateAsync(EmployeeModel emp)
        {
            var errors = new List<string>();

            var validateErrors = EmployeeValidator.ValidateCreate(emp);
            errors.AddRange(validateErrors.Select(e => e.ToString()));

            if (!string.IsNullOrEmpty(emp.Email) && await _employeeRepository.EmailExistAsync(emp.Email))
            {
                errors.Add(EnumEmployeeValidationError.EmailExisted.ToString());
            }
            var exists = await _departmentRepository.ExistAsync(emp.DepartmentId);
            if (!exists)
            {
                errors.Add(EnumEmployeeValidationError.DepartmentNotExist.ToString());
            }
            if (errors.Any())
            {
                throw new ValidationException(errors);
            }
            return await _employeeRepository.CreateAsync(emp);
        }

        public async Task<IEnumerable<GetTopSalaryQueryModel>> GetTopAsync(int topN)
        {
            if (topN <= 0)
            {
                throw new ValidationException(new List<string>
                {
                    "Top must be greater than 0"
                });
            }
            return await _employeeRepository.GetTopAsync(topN);
        }
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _employeeRepository.GetAllAsync();
        }
        public async Task<Employee?> GetByIdAsync(int id)
        {
            var emp = await _employeeRepository.GetByIdAsync(id);

            if (emp == null)
            {
                throw new ValidationException(new List<string>
                {
                    EnumEmployeeValidationError.EmployeeNotExisted.ToString()
                });
            }
            return emp;
        }
        //MethodResult
        //public async Task<MethodResult<int>> UpdateAsync(int id, UpdateEmployeeModel emp)
        //{
        //    var result = new MethodResult<int>();

        //    var validateErrors = EmployeeValidator.ValidateUpdate(emp);
        //     foreach (var error in validateErrors)
        //    {
        //        result.AddError(error.ToString());
        //    }
        //    var exists = await _employeeRepository.GetByIdAsync(id);
        //    if (exists == null)
        //    {
        //        result.AddError("EmployeeNotExisted", "Id", id);
        //    }
        //    if (emp.DepartmentId.HasValue)
        //    {
        //        var departmentCheck = await _departmentRepository.ExistAsync(emp.DepartmentId.Value);
        //        if (!departmentCheck)
        //        {
        //            result.AddError("DepartmentNotExisted", "DepartmentId", emp.DepartmentId);
        //        }
        //    }
        //    if (!result.IsOK)
        //        {
        //            return result;
        //        }
        //    var update = await _employeeRepository.UpdateAsync(id, emp);

        //    result.Result = update;
        //    result.StatusCode = 200;
        //    return result;
        //    }

        //Middleware

        public async Task<int> UpdateAsync(int id, UpdateEmployeeModel emp)
        {
            var errors = new List<string>();

            var validateErrors = EmployeeValidator.ValidateUpdate(emp);
            errors.AddRange(validateErrors.Select(e => e.ToString()));

            var exists = await _employeeRepository.GetByIdAsync(id);
            if (exists == null)
            {
                errors.Add(EnumEmployeeValidationError.EmployeeNotExisted.ToString());
            }
            if (emp.DepartmentId.HasValue)
            {
                var departmentCheck = await _departmentRepository.ExistAsync(emp.DepartmentId.Value);
                if (!departmentCheck)
                {
                    errors.Add(EnumEmployeeValidationError.DepartmentNotExist.ToString());
                }
            }
            if (errors.Any())
            {
                throw new ValidationException(errors);
            }
            return await _employeeRepository.UpdateAsync(id, emp);
        }
        public async Task<int> DeleteAsync(int id)
        {
            var exists = await _employeeRepository.GetByIdAsync(id);
            if (exists == null)
            {
                throw new ValidationException(new List<string>
                {
                    EnumEmployeeValidationError.EmployeeNotExisted.ToString()
                });
            }
            return await _employeeRepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<NewHireQueryModel>> GetNewHireMonthAsync(int year)
        {
            if (year <= 2000)
            {
                throw new ValidationException(new List<string>
                {
                    "Year must be greater than 2000"
                });
            }
            return await _employeeRepository.GetNewHireMonthAsync(year);
        }
    }
}
