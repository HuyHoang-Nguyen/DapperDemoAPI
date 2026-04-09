using System.ComponentModel.DataAnnotations.Schema;

namespace DapperDemoAPI.Common.Results
{
    [NotMapped]
    public class ErrorResult
    {
        public string? ErrorCode { get; set; }

        public IList<Error> Errors { get; set; }

        public ErrorResult()
        {
            Errors = new List<Error>();
        }
    }
}
