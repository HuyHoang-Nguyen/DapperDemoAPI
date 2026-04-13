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
                errors.Add("Month must be between 1 and 12");
            }

            if (year <= 2000)
            {
                errors.Add("Year must be greater than 2000");
            }

            if (errors.Any())
            {
                throw new ValidationException(errors);
            }
        }
    }
}
