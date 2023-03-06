using System;
using System.Collections.Generic;
using System.Text;

namespace Com.XpressPayments.Data.GenericResponse
{
    public class BaseResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public object Data { get; set; }
    }

    public class RequesterInfo
    {
        public string Username { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string IpAddress { get; set; }
        public string Port { get; set; }
    }
}
