using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpressHRMS.Data.Enums
{
   public enum ResponseCode
    {
        //[Description("Success")]
        //Ok = 00,
        //[Description("Validation Error")]
        //ValidationError = 01,
        //[Description("Not Found")]
        //NotFound = 02,
        //[Description("Bad Request")]
        //ProcessingError = 03,
        //[Description("Unauthorized Access")]
        //AuthorizationError = 04,
        //[Description("Duplicate Error")]
        //DuplicateError = 05,
        //[Description("Pending")]
        //Pending = 06,
        //[Description("Exception Occurred")]
        //Exception = 07,
        //[Description("Internal Server Error")]
        //InternalServer = 08,
        //[Description("OTP Validation")]
        //OtpValidation = 09,
        //[Description("Invalid Request")]
        //INVALID_REQUEST = 10,
        //[Description("Already Exist")]
        //Already_Exist = 11

        Ok = 00,
        NotFound = 02,
        ProcessingError = 03,
        AuthorizationError = 04,
        Already_Exist = 11,
        OtpValidation = 09,
        Pending = 06,
        InternalServer = 08,
        Exception=09,
        ValidationError=10
    }
}
