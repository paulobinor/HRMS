using hrms_be_backend_business.ILogic;
using hrms_be_backend_business.Logic;
using hrms_be_backend_common.Models;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace hrms_be_backend_api.LeaveModuleController.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveApprovalController : ControllerBase
    {
        private readonly ILogger<LeaveRequestController> _logger;
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly IAuthService _authService;

        public LeaveApprovalController(ILogger<LeaveRequestController> logger, ILeaveRequestService leaveRequestService, IAuthService authService)
        {
            _logger = logger;
            _leaveRequestService = leaveRequestService;
            _authService = authService;
        }

        [Authorize]
        [HttpGet("Info")]
        public async Task<IActionResult> GetLeaveApprovalInfo([FromQuery] long LeaveRequestLineItemId, [FromQuery] long leaveApprovalId)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            _logger.LogInformation($"Received GetLeaveApprovalInfo request. Payload: {JsonConvert.SerializeObject(new { LeaveRequestLineItemId, leaveApprovalId })} from remote address: {RemoteIpAddress}");

            var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
            var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

            }
            var res = await _leaveRequestService.GetLeaveApprovalInfo(leaveApprovalId, LeaveRequestLineItemId); 
            return Ok(res);
        }

        [HttpPost("Approve")]
        [Authorize]
        public async Task<IActionResult> ApproveLeaveRequestLineItem(LeaveApprovalLineItem leaveApprovalLineItem)
        {
            var response = new BaseResponse();
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            _logger.LogInformation($"Received ApproveLeave request. Payload: {JsonConvert.SerializeObject(leaveApprovalLineItem)} from remote address: {RemoteIpAddress}");

            var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
            var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

            }
            var res = await _leaveRequestService.UpdateLeaveApproveLineItem(leaveApprovalLineItem);
            return Ok(res);
        }

    }

}
