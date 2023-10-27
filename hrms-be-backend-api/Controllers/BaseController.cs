using hrms_be_backend_common.Communication;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace hrms_be_backend_api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IActionResult CustomResponse<T>(ExecutedResult<T> result)
        {
            ResponseCode.TryParse(result.responseCode, out ResponseCode myStatus);
            result.responseCode = result.responseCode.Length > 1 ? result.responseCode : '0' + result.responseCode;
            switch (myStatus)
            {               
                case ResponseCode.NotAuthenticated:
                    return Unauthorized(result);               
                default:
                    return Ok(result);
            }
        }

        protected IActionResult CustomResponse(BaseResponse result)
        {
            ResponseCode.TryParse(result.ResponseCode, out ResponseCode myStatus);
            result.ResponseCode = result.ResponseCode.Length > 1 ? result.ResponseCode : '0' + result.ResponseCode;
            switch (myStatus)
            {
                case ResponseCode.NotAuthenticated:
                    return Unauthorized(result);
                default:
                    return Ok(result);
            }
        }
    }
}

