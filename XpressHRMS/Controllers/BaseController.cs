using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using XpressHRMS.Business.GenericResponse;
using System.Linq;
using System.Threading.Tasks;
using XpressHRMS.Data.Enums;

namespace XpressHRMS.Controllers
{
    public class BaseController : ControllerBase
    {
        public IActionResult CustomResponse<T>(BaseResponse<T> result)
        {
            ResponseCode.TryParse(result.ResponseCode, out ResponseCode myStatus);
            result.ResponseCode = result.ResponseCode.Length > 1 ? result.ResponseCode : '0' + result.ResponseCode;
            switch (myStatus)
            {
                case ResponseCode.ProcessingError:
                    return Ok(result);
                case ResponseCode.AuthorizationError:
                    return Ok(result);
                case ResponseCode.NotFound:
                    return Ok(result);
                default:
                    return Ok(result);
            }
        }

    //    public class BaseResponse
    //    {
    //        public string ResponseCode { get; set; }
    //        public string ResponseMessage { get; set; }
    //        public object Data { get; set; }
    //    }
    //    public class BaseResponseLogin
    //    {
    //        public object Data { get; set; }
    //        public object jwttoken { get; set; }
    //        public string RoleName { get; set; }
    //        public int CompanyID { get; set; }
    //        public string ResponseCode { get; set; }
    //        public string ResponseMessage { get; set; }
    //    }
    //    public class SSOLogout
    //    {
    //        public string responseCode { get; set; }
    //        public string responseMessage { get; set; }
    //        public object data { get; set; }
    //    }
    }
}
