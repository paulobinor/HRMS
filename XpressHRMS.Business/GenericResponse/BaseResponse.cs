using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpressHRMS.Business.GenericResponse
{
   public class BaseResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public object Data { get; set; }
    }
    public class BaseResponseLogin
    {
        public object Data { get; set; }
        public object jwttoken { get; set; }
        public string RoleName { get; set; }
        public int CompanyID { get; set; }
    }
}
