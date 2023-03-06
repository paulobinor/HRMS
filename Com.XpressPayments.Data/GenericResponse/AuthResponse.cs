using System;
using System.Collections.Generic;
using System.Text;

namespace Com.XpressPayments.Data.GenericResponse
{
    public class AuthResponse
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
        public string Message { get; set; }
    }
}
