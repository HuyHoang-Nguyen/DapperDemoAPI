using DapperDemoAPI.Enums.EnumError;
using DapperDemoAPI.GlobalExceptionHandler;

namespace DapperDemoAPI.Validators
{
    public class PayrollValidator
    {
        public static void ValidatePayroll(int month, int year)
        {
            var errors = new List<string>();

            if (month < 1 || month > 12)
            {
                errors.Add(EnumPayrollValidatorError.MonthInvalid.ToString());
            }

            if (year <= 2000)
            {
                errors.Add(EnumPayrollValidatorError.YearInvalid.ToString());
            }

            if (errors.Any())
            {
                throw new ValidationException(errors);
            }
        }
    }
}
