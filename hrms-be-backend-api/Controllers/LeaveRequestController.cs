using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hrms_be_backend_api.LeaveModuleController.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILogger<LeaveRequestController> _logger;
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILogger<LeaveRequestController> logger, ILeaveRequestService leaveRequestService)
        {
            _logger = logger;
            _leaveRequestService = leaveRequestService;
        }
        [HttpPost("CreateLeaveRequest")]
        [Authorize]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] LeaveRequestCreate CreateDto)
        {
            var response = new BaseResponse();
            var requester = new RequesterInfo
            {
                Username = this.User.Claims.ToList()[2].Value,
                UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                Port = Request.HttpContext.Connection.LocalPort.ToString()
            };

            return Ok(await _leaveRequestService.CreateLeaveRequest(CreateDto, requester));
        }

        [HttpPost("RescheduleLeaveRequest")]
        [Authorize]
        public async Task<IActionResult> RescheduleLeaveRequest([FromBody] RescheduleLeaveRequest updateDto)
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

                return Ok(await _leaveRequestService.RescheduleLeaveRequest(updateDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : RescheduleLeaveRequest ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : RescheduleLeaveRequest ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }
        [HttpPost("ApproveLeaveRequest")]
        [Authorize]
        public async Task<IActionResult> ApproveLeaveRequest( long LeaveRequestID)
        {
            var response = new BaseResponse();
            var requester = new RequesterInfo
            {
                Username = this.User.Claims.ToList()[2].Value,
                UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                Port = Request.HttpContext.Connection.LocalPort.ToString()
            };

            return Ok(await _leaveRequestService.ApproveLeaveRequest(LeaveRequestID, requester));
        }
        [HttpPost("DisaproveLeaveRequest")]
        [Authorize]
        public async Task<IActionResult> DisaproveLeaveRequest([FromBody] LeaveRequestDisapproved payload)
        {
            var response = new BaseResponse();
            var requester = new RequesterInfo
            {
                Username = this.User.Claims.ToList()[2].Value,
                UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                Port = Request.HttpContext.Connection.LocalPort.ToString()
            };

            return Ok(await _leaveRequestService.DisaproveLeaveRequest(payload, requester));
        }

        [Authorize]
        [HttpGet("GetAllLeaveRequest")]
        public async Task<IActionResult> GetAllLeaveRequest()
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

                return Ok(await _leaveRequestService.GetAllLeaveRquest(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllLeaveRequest ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllLeaveRequest ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetLeaveRequestbyId")]
        public async Task<IActionResult> GetLeaveRequestbyId(long LeaveRequestID)
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

                return Ok(await _leaveRequestService.GetLeaveRequsetById(LeaveRequestID,  requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetLeaveRequestbyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetLeaveRequestbyId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [Authorize]
        [HttpGet("GetLeaveRequestbyUserId")]
        public async Task<IActionResult> GetLeaveRequestbyUserId(long UserId, long CompanyId)
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

                return Ok(await _leaveRequestService.GetLeaveRequsetByUerId(UserId, CompanyId, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetLeaveRequsetByUerId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetLeaveRequsetByUerId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [Authorize]
        [HttpGet("GetLeaveRequestbyCompanyId")]
        public async Task<IActionResult> GetLeaveRequestbyCompanyId(string RequestYear, long CompanyID)
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

                return Ok(await _leaveRequestService.GetLeaveRquestbyCompanyId(RequestYear,CompanyID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetLeaveTypebyCompanyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetLeaveTypebyCompanyId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }
        [Authorize]
        [HttpGet("GetLeaveRequestPendingApproval")]
        public async Task<IActionResult> GetLeaveRequestPendingApproval()
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

                return Ok(await _leaveRequestService.GetLeaveRequestPendingApproval(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllLeaveRequest ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetLeaveRequestPendingApproval ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }
    }
}
