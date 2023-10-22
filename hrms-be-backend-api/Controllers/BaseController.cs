using hrms_be_backend_common.Communication;
using hrms_be_backend_data.Enums;
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
                case ResponseCode.AuthorizationError:
                    return Unauthorized(result);               
                default:
                    return Ok(result);
            }
        }
    }
}

