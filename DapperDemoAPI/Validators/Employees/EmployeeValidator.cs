using DapperDemoAPI.Enums.EnumError;
using DapperDemoAPI.Models.Employee;

namespace DapperDemoAPI.Validators.Employees
{
        public static class EmployeeValidator
        {
            public static List<EnumEmployeeValidationError> ValidateCommon(string? fullName, string? email, string? phone, DateOnly? hireDate, decimal? baseSalary)
            {
                var errors = new List<EnumEmployeeValidationError>();

                if (string.IsNullOrWhiteSpace(fullName))
                {
                    errors.Add(EnumEmployeeValidationError.NameRequired);
                }
                if (string.IsNullOrWhiteSpace(email))
                {
                    errors.Add(EnumEmployeeValidationError.EmailRequired);
                }
                else if (!email.Contains("@") || !email.Contains("."))
                {
                    errors.Add(EnumEmployeeValidationError.EmailInvalid);
                }
                if (!string.IsNullOrWhiteSpace(phone) && phone.Length > 12)
                {
                    errors.Add(EnumEmployeeValidationError.PhoneInvalid);
                }
                if (hireDate.HasValue && hireDate.Value >= DateOnly.FromDateTime(DateTime.Today))
                {
                    errors.Add(EnumEmployeeValidationError.HireDateInvalid);
                }
                if (baseSalary.HasValue && baseSalary.Value <= 0)
                {
                    errors.Add(EnumEmployeeValidationError.SalaryInvalid);
                }
                return errors;
            }
            public static List<EnumEmployeeValidationError> ValidateCreate(EmployeeModel emp)
            {
                return ValidateCommon(emp.FullName, emp.Email, emp.Phone, emp.HireDate, emp.BaseSalary);
            }
            public static List<EnumEmployeeValidationError> ValidateUpdate(UpdateEmployeeModel emp)
            {
                var errors = new List<EnumEmployeeValidationError>();
                if (emp.FullName != null)
                {
                    if (string.IsNullOrWhiteSpace(emp.FullName))
                    {
                        errors.Add(EnumEmployeeValidationError.NameRequired);
                    }
                }
                if (emp.Phone != null)
                {
                    if (string.IsNullOrWhiteSpace(emp.Phone) || emp.Phone.Length > 11)
                    {
                        errors.Add(EnumEmployeeValidationError.PhoneInvalid);
                    }
                }
                if (emp.BaseSalary.HasValue && emp.BaseSalary <= 0)
                {
                    errors.Add(EnumEmployeeValidationError.SalaryInvalid);
                }
                return errors;
            }
        }
    }