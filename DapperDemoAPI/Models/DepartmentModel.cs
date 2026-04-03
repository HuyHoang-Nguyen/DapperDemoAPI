using System.ComponentModel.DataAnnotations;

namespace DapperDemoAPI.Models
{
    public class DepartmentModel
    {
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string? Name { get; set; }
    }
}
