using System.ComponentModel.DataAnnotations;

namespace DapperDemoAPI.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}
