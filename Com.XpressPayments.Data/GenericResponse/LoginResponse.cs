using System;
using System.Collections.Generic;
using System.Text;

namespace Com.XpressPayments.Data.GenericResponse
{
    public class LoginResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
        public object Data { get; set; }
    }

    public class RefreshTokenResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
