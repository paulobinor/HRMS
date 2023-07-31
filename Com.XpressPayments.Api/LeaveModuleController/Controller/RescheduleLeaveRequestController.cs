using Com.XpressPayments.Bussiness.LeaveModuleService.Service.ILogic;
using Com.XpressPayments.Bussiness.LeaveModuleService.Service.Logic;
using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.LeaveModuleDTO.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System;
using Com.XpressPayments.Data.Enums;

namespace Com.XpressPayments.Api.LeaveModuleController.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class RescheduleLeaveRequestController : ControllerBase
    {
        private readonly ILogger<RescheduleLeaveRequestController> _logger;
        private readonly IRescheduleLeaveService _RescheduleLeaveService;

        public RescheduleLeaveRequestController(ILogger<RescheduleLeaveRequestController> logger, IRescheduleLeaveService RescheduleLeaveService)
        {
            _logger = logger;
            _RescheduleLeaveService = RescheduleLeaveService;
        }

        [HttpPost("CreateRescheduleLeaveRequest")]
        [Authorize]
        public async Task<IActionResult> CreateRescheduleLeaveRequest([FromBody] RescheduleLeaveRequestCreate CreateDto)
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

            return Ok(await _RescheduleLeaveService.CreateRescheduleLeaveRequest(CreateDto, requester));
        }

        [HttpPost("ApproveRescheduleLeaveRequest")]
        [Authorize]
        public async Task<IActionResult> ApproveRescheduleLeaveRequest([FromBody] long RescheduleLeaveRequestID)
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

            return Ok(await _RescheduleLeaveService.ApproveRescheduleLeaveRequest(RescheduleLeaveRequestID, requester));
        }

        [HttpPost("DisaproveRescheduleLeaveRequest")]
        [Authorize]
        public async Task<IActionResult> DisaproveRescheduleLeaveRequest([FromBody] RescheduleLeaveRequestDisapproved payload)
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

            return Ok(await _RescheduleLeaveService.DisaproveRescheduleLeaveRequest(payload, requester));
        }

        [Authorize]
        [HttpGet("GetAllRescheduleLeaveRequest")]
        public async Task<IActionResult> GetAllRescheduleLeaveRequest()
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

                return Ok(await _RescheduleLeaveService.GetAllRescheduleLeaveRquest(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllRescheduleLeaveRequest ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllRescheduleLeaveRequest ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetRescheduleLeaveRequestbyId")]
        public async Task<IActionResult> GetRescheduleLeaveRequestbyId(long RescheduleLeaveRequestID)
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

                return Ok(await _RescheduleLeaveService.GetRescheduleLeaveRequsetById(RescheduleLeaveRequestID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetRescheduleLeaveRequsetById ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetRescheduleLeaveRequsetById ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [Authorize]
        [HttpGet("GetRescheduleLeaveRequestbyCompanyId")]
        public async Task<IActionResult> GetRescheduleLeaveRequestbyCompanyId(string RequestYear, long CompanyID)
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

                return Ok(await _RescheduleLeaveService.GetRescheduleLeaveRequestbyCompanyId(RequestYear, CompanyID, requester));
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
        [HttpGet("GetRescheduleLeaveRequestPendingApproval")]
        public async Task<IActionResult> GetRescheduleLeaveRequestPendingApproval()
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

                return Ok(await _RescheduleLeaveService.GetRescheduleLeaveRequestPendingApproval(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetRescheduleLeaveRequestPendingApproval ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetRescheduleLeaveRequestPendingApproval ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }


    }
}
