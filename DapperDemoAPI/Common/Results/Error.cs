using System.ComponentModel.DataAnnotations.Schema;

namespace DapperDemoAPI.Common.Results
{
    [NotMapped]
    public class Error
    {
        public string? FieldName { get; set; }

        private IList<object>? P_ErrorValues { get; set; }

        public IList<object>? ErrorValues
        {
            get
            {
                return P_ErrorValues;
            }
            set
            {
                P_ErrorValues = value;
            }
        }

        public IList<object>? ExactValues { get; set; }

        public Error()
        {
            ErrorValues = new List<object>();
            ExactValues = new List<object>();
        }

        public Error(string? fieldName)
        {
            FieldName = fieldName;
        }

        public Error(object? errorValue)
        {
            if (errorValue != null)
            {
                ErrorValues = new List<object> { errorValue };
            }
        }

        public Error(string? fieldName, object? errorValue)
        {
            FieldName = fieldName;
            if (errorValue != null)
            {
                ErrorValues = new List<object> { errorValue };
            }
        }

        public Error(string? fieldName, IList<object>? errorValues)
        {
            FieldName = fieldName;
            ErrorValues = errorValues;
        }

        public Error(string? fieldName, IList<object>? errorValues, IList<object>? exactValues)
        {
            FieldName = fieldName;
            ErrorValues = errorValues;
            ExactValues = exactValues;
        }
    }
}
