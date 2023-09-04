using Com.XpressPayments.Bussiness.LeaveModuleService.Service.ILogic;
using Com.XpressPayments.Bussiness.LeaveModuleService.Service.Logic;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.LeaveModuleDTO.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;

namespace Com.XpressPayments.Api.LeaveModuleController.Controller
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
        [HttpPost("ApproveLeaveRequest")]
        [Authorize]
        public async Task<IActionResult> ApproveLeaveRequest([FromBody] long LeaveRequestID)
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
        public async Task<IActionResult> GetLeaveRequestbyId(long LeaveRequestID, long CompanyId)
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
