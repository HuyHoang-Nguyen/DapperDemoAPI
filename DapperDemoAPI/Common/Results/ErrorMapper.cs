using DapperDemoAPI.Enums.EnumError;

namespace DapperDemoAPI.Common.Results
{
    public static class ErrorMapper
    {
        public static void Map(this VoidMethodResult result, EnumEmployeeValidationError errorCode)
        {
            switch (errorCode)
            {
                case EnumEmployeeValidationError.EmailExisted: 
                    result.AddErrorBadRequest("EmailExisted", "Email");
                    break;
                case EnumEmployeeValidationError.DepartmentNotExist:
                    result.AddErrorBadRequest("DepartmentNotExist", "Department");
                    break;
                case EnumEmployeeValidationError.NameRequired:
                    result.AddErrorBadRequest("NameRequired", "FullName");
                    break;
                default:
                    result.AddErrorBadRequest(errorCode.ToString());
                    break;
            }
        }
    }
}
