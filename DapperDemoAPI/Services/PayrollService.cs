using DapperDemoAPI.Enums.EnumError;
using DapperDemoAPI.GlobalExceptionHandler;
using DapperDemoAPI.IRepositories;
using DapperDemoAPI.QueryModels;
using DapperDemoAPI.Services.Interfaces;
using DapperDemoAPI.Validators;

namespace DapperDemoAPI.Services
{
    public class PayrollService : IPayrollService
    {
        private readonly IPayrollRepository _payrollRepository;

        public PayrollService(IPayrollRepository payrollRepository)
        {
            _payrollRepository = payrollRepository;
        }
        public async Task<int> InsertPayrollAsync(int month, int year)
        {
            PayrollValidator.ValidatePayroll(month, year);
            return await _payrollRepository.InsertPayrollAsync(month, year);
        }
        public async Task<GetPayrollReportModel?> GetPayrollReportAsync(int month, int year)
        {
            PayrollValidator.ValidatePayroll(month, year);
            var result = await _payrollRepository.GetPayrollReportAsync(month, year);
            if (result == null)
            {
                throw new ValidationException(new List<string>
                {
                    EnumPayrollValidatorError.DataNotFound.ToString()
                });
            }
            return result;
        }
    }
}
