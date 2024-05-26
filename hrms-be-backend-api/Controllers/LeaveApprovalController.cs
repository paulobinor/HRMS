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
        private readonly ILogger<LeaveApprovalController> _logger;
        private readonly ILeaveApprovalService _leaveApprovalService;
        private readonly IAuthService _authService;

        public LeaveApprovalController(ILogger<LeaveApprovalController> logger, ILeaveApprovalService leaveApprovalService, IAuthService authService)
        {
            _logger = logger;
            _leaveApprovalService = leaveApprovalService;
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
            var res = await _leaveApprovalService.GetLeaveApprovalInfo(leaveApprovalId, LeaveRequestLineItemId);
            return Ok(new BaseResponse { Data = res, ResponseCode = "00", ResponseMessage = "Success" });
        }

        [HttpPost("Approve")]
      //  [Authorize]
        public async Task<IActionResult> ApproveLeaveRequestLineItem(LeaveApprovalLineItem leaveApprovalLineItem)
        {
            var response = new BaseResponse();
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            _logger.LogInformation($"Received Approve Leave request. Payload: {JsonConvert.SerializeObject(leaveApprovalLineItem)} from remote address: {RemoteIpAddress}");

            var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
            var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

            }
            var res = await _leaveApprovalService.UpdateLeaveApproveLineItem(leaveApprovalLineItem);
            return Ok(res);
        }

      

        [HttpGet("GetAllLeaveApprovalLineItems")]
        [Authorize]
        public async Task<IActionResult> GetAllLeaveApprovalLineItems( [FromQuery] long CompanyID)
        {
            var response = new BaseResponse();
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            _logger.LogInformation($"Received GetAllLeaveApprovalLineItems request. Payload: {JsonConvert.SerializeObject(new { CompanyID })} from remote address: {RemoteIpAddress}");

            var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
            var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

            }
            var res = await _leaveApprovalService.GetLeaveApprovalInfoByCompanyID(CompanyID);
            response.Data = res;
            response.ResponseMessage = "Success";
            response.ResponseCode = "00";
            return Ok(response);
        }

        [HttpGet("GetPendingLeaveApprovals")]
        [Authorize]
        public async Task<IActionResult> GetPendingLeaveApprovals([FromQuery] long ApprovalEmployeeID)
        {
            var response = new BaseResponse();
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            _logger.LogInformation($"Received GetPendingLeaveApprovals request. Payload: {JsonConvert.SerializeObject(new { ApprovalEmployeeID })} from remote address: {RemoteIpAddress}");

            var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
            var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

            }
            var res = await _leaveApprovalService.GetPendingLeaveApprovals(ApprovalEmployeeID, "Pending");
            response.Data = res;
            response.ResponseMessage = "Success";
            response.ResponseCode = "00";
            return Ok(response);
        }

        [HttpGet("GetLeaveApprovals")]
      //  [Authorize]
        public async Task<IActionResult> GetLeaveApprovals([FromQuery] long ApprovalEmployeeID)
        {
            var response = new BaseResponse();
            //var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            //_logger.LogInformation($"Received GetPendingLeaveApprovals request. Payload: {JsonConvert.SerializeObject(new { ApprovalEmployeeID })} from remote address: {RemoteIpAddress}");

            //var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
            //var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
            //if (accessUser.data == null)
            //{
            //    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

            //}
            var res = await _leaveApprovalService.GetPendingLeaveApprovals(ApprovalEmployeeID);
            response.Data = res;
            response.ResponseMessage = "Success";
            response.ResponseCode = "00";
            return Ok(response);
        }

       

    }

}
