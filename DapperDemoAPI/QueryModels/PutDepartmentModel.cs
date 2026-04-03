using System.ComponentModel.DataAnnotations;

namespace DapperDemoAPI.QueryModels
{
    public class PutDepartmentModel
    {
        [MaxLength(200)]
        public string? Name { get; set; }
    }
}
