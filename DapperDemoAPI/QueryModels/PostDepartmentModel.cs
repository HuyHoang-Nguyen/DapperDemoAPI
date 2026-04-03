using System.ComponentModel.DataAnnotations;

namespace DapperDemoAPI.QueryModels
{
    public class PostDepartmentModel
    {
        [Required, MaxLength(200)]
        public string? Name { get; set; }
    }
}
