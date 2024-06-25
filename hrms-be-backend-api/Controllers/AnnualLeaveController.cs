using hrms_be_backend_business.ILogic;
using hrms_be_backend_business.Logic;
using hrms_be_backend_common.DTO;
using hrms_be_backend_common.Models;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace hrms_be_backend_api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnualLeaveController : ControllerBase
    {
        private readonly ILogger<AnnualLeaveController> _logger;
        private readonly ILeaveApprovalService _leaveApprovalService;
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly IAuthService _authService;

        public AnnualLeaveController(ILogger<AnnualLeaveController> logger, ILeaveApprovalService leaveApprovalService, IAuthService authService, ILeaveRequestService leaveRequestService)
        {
            _logger = logger;
            _leaveApprovalService = leaveApprovalService;
            _authService = authService;
            _leaveRequestService = leaveRequestService;
        }

      //  [Authorize]
        [HttpPost("CreateMultiple")]
        public async Task<ActionResult<List<LeaveRequestLineItem>>> CreateLeaveRequest([FromBody] List<CreateLeaveRequestLineItem> leaveRequests)
        {
            //var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            //_logger.LogInformation($"Received Create leave request. Payload: {JsonConvert.SerializeObject(leaveRequests)} from remote address: {RemoteIpAddress}");
            //var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();

            //if (string.IsNullOrEmpty(accessToken))
            //{
            //    return BadRequest(new { responseMessage = $"Missing authorization header value", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });
            //}
            //var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
            //if (accessUser.data == null)
            //{
            //    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });
            //}
            List<LeaveRequestLineItem> requestLineItems = new List<LeaveRequestLineItem>();
            LeaveRequestLineItem requestLineItem = null;
            foreach (var create in leaveRequests)
            {
                requestLineItem = new LeaveRequestLineItem
                {
                    CompanyId = create.CompanyId,
                    LeaveTypeId = create.LeaveTypeId,
                    EmployeeId = create.EmployeeId,
                    endDate = create.endDate,
                    startDate = create.startDate,
                    HandoverNotes = create.HandoverNotes,
                    IsApproved = false,
                    IsRescheduled = false,
                    LeaveLength = create.LeaveLength,
                    ResumptionDate = create.ResumptionDate,
                    RelieverUserId = create.RelieverUserId,
                    UploadFilePath = create.UploadFilePath
                };
                requestLineItems.Add(requestLineItem);
            }
            var res = await _leaveRequestService.CreateLeaveRequestLineItem(requestLineItems);
            return Ok(res);
        }

        [HttpPost("RescheduleAnnual")]
       // [Authorize]
        public async Task<IActionResult> Reschedule([FromBody] List<LeaveRequestLineItem> leaveRequestLineItems)
        {
            var response = new BaseResponse();
            try
            {
                //var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                //_logger.LogInformation($"Received request to reschedule annual leave. Payload: {JsonConvert.SerializeObject(leaveRequestLineItems)} from remote address: {RemoteIpAddress}");

                //var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();

                //var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                //if (accessUser.data == null)
                //{
                //    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });
                //}
                var res = await _leaveRequestService.RescheduleAnnualLeaveRequest(leaveRequestLineItems);
                return Ok(res);
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


        [Authorize]
        [HttpGet("GetAnnualLeaveRequests")]
        public async Task<IActionResult> GetAnnualLeaveRequestLineItems([FromQuery] string CompanyID)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                _logger.LogInformation($"Received GetAllLeaveRequestLineItems. Payload: {JsonConvert.SerializeObject(new { CompanyID })} from remote address: {RemoteIpAddress}");

                var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
                var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

                }
                var res = await _leaveRequestService.GetEmpAnnualLeaveRquestLineItems(Convert.ToInt64(CompanyID));


                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: Controller Method : GetAllLeaveRequestLineItems ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllLeaveRequest ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        //[Authorize]
        [HttpGet("GetEmpAnnualLeaveInfo")]
        public async Task<IActionResult> GetAnnualLeaveInfo([FromQuery] long CompanyId, [FromQuery] long EmployeeId)
        {
            var response = new BaseResponse();
            try
            {
                //var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                //_logger.LogInformation($"Received GetEmpLeaveInfo request. Payload: {JsonConvert.SerializeObject(new { CompanyId, EmployeeId })} from remote address: {RemoteIpAddress}");

                //var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
                //var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                //if (accessUser.data == null)
                //{
                //    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

                //}
                var res = await _leaveRequestService.GetEmpAnnualLeaveInfoList(EmployeeId, CompanyId);
                if (res != null && res.Count > 0)
                {
                    response.Data = res;
                  //  return Ok(new BaseResponse { Data = res, ResponseCode = "00", ResponseMessage = "Success" });

                }
                response.ResponseCode = "00";
                response.ResponseMessage = "Success";
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAnnualLeaveInfo ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetLeaveRequestbyId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }


        [HttpPost("UpdateAnnualLeave")]
        //[Authorize]
        public async Task<IActionResult> ApproveAnnualLeave(LeaveApprovalLineItem leaveApprovalLineItem)
        {
            _logger.LogInformation($"recieved request to update leaveapproval by approval Id: {leaveApprovalLineItem.ApprovalEmployeeId}  payload is: {JsonConvert.SerializeObject(leaveApprovalLineItem)}");
            var response = new BaseResponse();
            //var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            //_logger.LogInformation($"Received Approve Leave request. Payload: {JsonConvert.SerializeObject(leaveApprovalLineItem)} from remote address: {RemoteIpAddress}");

            //var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
            //var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
            //if (accessUser.data == null)
            //{
            //    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

            //}
            //var leaveapprovals = await _leaveApprovalService.GetleaveApprovalLineItems(leaveApprovalLineItem.LeaveApprovalId);
            //foreach (var item in leaveapprovals)
            //{
            //    item.ApprovalStatus = leaveApprovalLineItem.ApprovalStatus;
            //    item.EntryDate = leaveApprovalLineItem.EntryDate;
            //    item.Comments = leaveApprovalLineItem.Comments;
            //   // item.IsApproved = leaveApprovalLineItem.IsApproved;
            //    item.EntryDate = leaveApprovalLineItem.EntryDate;
            //    item.ApprovalStep = leaveApprovalLineItem.ApprovalStep;
            //}
            var res = await _leaveApprovalService.UpdateAnnualLeaveApproval(leaveApprovalLineItem);
            return Ok(res);
        }

        [HttpGet("GetAnnualLeaveApprovals")]
      //  [Authorize]
        public async Task<IActionResult> GetAnnualLeaveApprovals([FromQuery] long ApprovalEmployeeID)
        {
            var response = new BaseResponse();
            //var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            //_logger.LogInformation($"Received GetAnnualLeaveApprovals request. Payload: {JsonConvert.SerializeObject(new { ApprovalEmployeeID })} from remote address: {RemoteIpAddress}");

            //var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
            //var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
            //if (accessUser.data == null)
            //{
            //    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

            //}
            var res = await _leaveApprovalService.GetPendingAnnualLeaveApprovals(ApprovalEmployeeID);
           
            response.Data = res;
            response.ResponseMessage = "Success";
            response.ResponseCode = "00";
            return Ok(response);
        }
    }
}
