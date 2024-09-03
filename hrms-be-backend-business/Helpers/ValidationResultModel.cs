using Microsoft.AspNetCore.Mvc;

namespace hrms_be_backend_business.Helpers
{
    public class ValidationResultModel
    {
        public bool IsValid { get; set; } = true;

        public ProblemDetails customProblemDetail { get; set; }
    }

    
}
