using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Models;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace hrms_be_backend_api.LeaveModuleController.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveApprovalController : ControllerBase
    {
        private readonly ILogger<LeaveRequestController> _logger;
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveApprovalController(ILogger<LeaveRequestController> logger, ILeaveRequestService leaveRequestService)
        {
            _logger = logger;
            _leaveRequestService = leaveRequestService;
        }

        //[Authorize]
        [HttpGet("Info")]
        public async Task<IActionResult> GetLeaveApprovalInfo([FromQuery] long LeaveRequestId, [FromQuery] long leaveApprovalId)
        {
            _logger.LogInformation($"Received request to get Leave Approval info for LeaveRequestId: {LeaveRequestId}");
            //var response = new BaseResponse();
            //var requester = new RequesterInfo
            //{
            //    Username = this.User.Claims.ToList()[2].Value,
            //    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
            //    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
            //    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
            //    Port = Request.HttpContext.Connection.RemotePort.ToString()
            //};
            var res = await _leaveRequestService.GetLeaveApprovalInfo(LeaveRequestId, leaveApprovalId); 
            return Ok(res);
        }

        [HttpPost("Approve")]
        //[Authorize]
        public async Task<IActionResult> ApproveLeaveRequestLineItem(LeaveApprovalLineItem leaveApprovalLineItem)
        {
            var response = new BaseResponse();
            var requester = new RequesterInfo
            {
                Username = this.User.Claims.ToList()[2].Value,
                UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                Port = Request.HttpContext.Connection.RemotePort.ToString()
            };
            var res = await _leaveRequestService.ApproveLeaveRequest(leaveApprovalLineItem);
            return Ok(res);
        }

    }
}
