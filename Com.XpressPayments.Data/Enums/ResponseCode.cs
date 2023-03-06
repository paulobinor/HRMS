using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Com.XpressPayments.Data.Enums
{
    public enum ResponseCode
    {
        [Description("Success")]
        Ok = 0,
        [Description("Validation Error")]
        ValidationError = 1,
        [Description("Not Found")]
        NotFound = 2,
        [Description("Bad Request")]
        ProcessingError = 3,
        [Description("Unauthorized Access")]
        AuthorizationError = 4,
        [Description("Duplicate Error")]
        DuplicateError = 5,
        [Description("Pending")]
        Pending = 6,
        [Description("Exception Occurred")]
        Exception = 7,
        [Description("Internal Server Error")]
        InternalServer = 8,
        [Description("Invalid Password")]
        InvalidPassword = 9,
    }
}
