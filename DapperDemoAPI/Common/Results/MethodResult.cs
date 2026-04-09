using Microsoft.AspNetCore.Mvc;

namespace DapperDemoAPI.Common.Results
{
    public class MethodResult<T> : VoidMethodResult
    {
        public T? Result { get; set; }

        public void AddResultFromErrorList(IEnumerable<ErrorResult>? errorMessages)
        {
            if (errorMessages == null)
            {
                return;
            }

            foreach (ErrorResult errorMessage in errorMessages)
            {
                AddError(errorMessage);
            }
        }
        public override IActionResult GetActionResult()
        {
            ObjectResult objectResult = new ObjectResult(this);
            if (!base.StatusCode.HasValue)
            {
                if (base.IsOK)
                {
                    objectResult.StatusCode = ((Result != null) ? 200 : 204);
                }
                else
                {
                    objectResult.StatusCode = 500;
                }

                return objectResult;
            }

            objectResult.StatusCode = base.StatusCode;
            return objectResult;
        }
    }
}
