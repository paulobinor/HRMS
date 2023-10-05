using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace hrms_be_backend_api.ExitModuleController.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResignationInterviewController : ControllerBase
    {
        private readonly ILogger<ResignationInterviewController> _logger;
        private readonly IResignationInterviewService _resignationInterviewService;

        public ResignationInterviewController(ILogger<ResignationInterviewController> logger, IResignationInterviewService resignationInterviewService)
        {
            _logger = logger;
            _resignationInterviewService = resignationInterviewService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitResignationInterview(ResignationInterviewVM request)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _resignationInterviewService.SubmitResignationInterview(requester, request));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : SubmitResignationInterview() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : SubmitResignationInterview() ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetInterviewScaleDetails()
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _resignationInterviewService.GetInterviewScaleDetails(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetInterviewScaleDetails() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetInterviewScaleDetails() ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("GetResignationInterview/{SRFID}")]
        public async Task<IActionResult> GetResignationInterview(long SRFID)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _resignationInterviewService.GetResignationInterview(SRFID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetInterviewDetails() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetInterviewDetails() ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("GetResignationInterviewDetails/{InterviewID}")]
        public async Task<IActionResult> GetResignationInterviewDetails(long InterviewID)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _resignationInterviewService.GetResignationInterviewDetails(InterviewID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetInterviewDetails() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetInterviewDetails() ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("ApprovePendingResignationInterview")]
        public async Task<IActionResult> ApprovePendingResignationInterview([FromBody] ApproveResignationInterviewDTO request)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _resignationInterviewService.ApprovePendingResignationInterview(request, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : ApprovePendingResignationInterview ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : ApprovePendingResignationInterview ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [HttpPost]
        [Route("DisapprovePendingResignationInterview")]
        public async Task<IActionResult> DisapprovePendingResignationInterview([FromBody] DisapproveResignationInterviewDTO request)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _resignationInterviewService.DisapprovePendingResignationInterview(request, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DisapprovePendingResignationInterview ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DisapprovePendingResignationInterview ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [HttpGet]
        [Route("GetAllApprovedResignationInterviewSwitch/{UserID}/{isApproved}")]
        public async Task<IActionResult> GetAllApprovedResignationInterview(long UserID, bool isApproved)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _resignationInterviewService.GetAllApprovedResignationInterview(UserID, isApproved, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllApprovedResignationInterview() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllApprovedResignationInterview() ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }
    }
}
