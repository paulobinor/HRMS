using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hrms_be_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaritalStatusController : ControllerBase
    {
        private readonly ILogger<MaritalStatusController> _logger;
        private readonly IMaritalStatusService _MaritalStatusService;

        public MaritalStatusController(ILogger<MaritalStatusController> logger, IMaritalStatusService MaritalStatusService)
        {
            _logger = logger;
            _MaritalStatusService = MaritalStatusService;
        }

        [Authorize]
        [HttpGet("GetAllMaritalStatus")]
        public async Task<IActionResult> GetAllMaritalStatus()
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _MaritalStatusService.GetAllMaritalStatus(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllMaritalStatus() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllMaritalStatus() ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }
    }
}
