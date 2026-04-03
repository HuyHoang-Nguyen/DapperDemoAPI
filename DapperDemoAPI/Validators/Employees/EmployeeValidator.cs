using DapperDemoAPI.Enums.EnumError;
using DapperDemoAPI.IRepositories;
using DapperDemoAPI.Models.Employee;
using FluentValidation;
using System;

namespace DapperDemoAPI.Validators.Employees
{
        public static class EmployeeValidator
        {
            public static List<EnumEmployeeValidationError> ValidateCommon(string? fullName, string? email, string? phone, DateOnly? hireDate, decimal? baseSalary)
            {
                var errors = new List<EnumEmployeeValidationError>();

                if (!string.IsNullOrWhiteSpace(fullName) == false)
                {
                    errors.Add(EnumEmployeeValidationError.NameRequired);
                }
                if (!string.IsNullOrWhiteSpace(email) == false)
                {
                    errors.Add(EnumEmployeeValidationError.EmailRequired);
                }
                else if (!email.Contains("@") || !email.Contains("."))
                {
                    errors.Add(EnumEmployeeValidationError.EmailInvalid);
                }
                if (!string.IsNullOrWhiteSpace(phone) && phone.Length > 11)
                {
                    errors.Add(EnumEmployeeValidationError.PhoneInvalid);
                }
                if (hireDate.HasValue && hireDate.Value > DateOnly.FromDateTime(DateTime.Today))
                {
                    errors.Add(EnumEmployeeValidationError.HireDateInvalid);
                }
                if (baseSalary.HasValue && baseSalary.Value < 0)
                {
                    errors.Add(EnumEmployeeValidationError.SalaryInvalid);
                }
                return errors;
            }
            public static async Task<List<EnumEmployeeValidationError>> ValidateCreateAsync(EmployeeModel emp, IEmployeeRepository repo)
            {
                var errors = ValidateCommon(emp.FullName, emp.Email, emp.Phone, emp.HireDate, emp.BaseSalary);
                if (string.IsNullOrWhiteSpace(emp.FullName))
                {
                    errors.Add(EnumEmployeeValidationError.NameRequired);
                }
                if (string.IsNullOrWhiteSpace(emp.Email))
                {
                    errors.Add(EnumEmployeeValidationError.EmailRequired);
                }
                if (!errors.Any())
                {
                    if (await repo.EmailExistAsync(emp.Email))
                    {
                        errors.Add(EnumEmployeeValidationError.EmailExisted);
                    }
                }
                return errors;
            }
            public static List<EnumEmployeeValidationError> ValidateUpdate(UpdateEmployeeModel e)
            {
                var errors = new List<EnumEmployeeValidationError>();

                if (e.FullName != null && string.IsNullOrWhiteSpace(e.FullName))
                {
                    errors.Add(EnumEmployeeValidationError.NameRequired);
                }
                if (e.Phone != null)
                {
                    if (string.IsNullOrWhiteSpace(e.Phone) || e.Phone.Length > 11)
                    {
                        errors.Add(EnumEmployeeValidationError.PhoneInvalid);
                    }
                }
                if (e.DepartmentId.HasValue && e.DepartmentId <= 0)
                {
                    errors.Add(EnumEmployeeValidationError.DepartmentInvalid);
                }
                if (e.BaseSalary.HasValue && e.BaseSalary < 0)
                {
                    errors.Add(EnumEmployeeValidationError.SalaryInvalid);
                }
                return errors;
            }
        }
    }