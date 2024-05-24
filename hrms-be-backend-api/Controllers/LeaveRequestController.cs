using hrms_be_backend_business.ILogic;
using hrms_be_backend_business.Logic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.Models;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Claims;
using static iText.Signatures.LtvVerification;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace hrms_be_backend_api.LeaveModuleController.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILogger<LeaveRequestController> _logger;
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly IAuthService _authService;

        public LeaveRequestController(ILogger<LeaveRequestController> logger, ILeaveRequestService leaveRequestService, IAuthService authService)
        {
            _logger = logger;
            _leaveRequestService = leaveRequestService;
            _authService = authService;
        }

      
        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] CreateLeaveRequestLineItem createLeaveRequestLine)
        {
            var leaveRequestLineItem = new LeaveRequestLineItem
            {
                CompanyId = createLeaveRequestLine.CompanyId,
                LeaveTypeId = createLeaveRequestLine.LeaveTypeId,
                EmployeeId = createLeaveRequestLine.EmployeeId,
                endDate = createLeaveRequestLine.endDate,
                startDate = createLeaveRequestLine.startDate,
                HandoverNotes = createLeaveRequestLine.HandoverNotes,
                IsApproved = false,
                IsRescheduled = false,
                LeaveLength = createLeaveRequestLine.LeaveLength,
                ResumptionDate = createLeaveRequestLine.ResumptionDate,
                RelieverUserId = createLeaveRequestLine.RelieverUserId,
                UploadFilePath = createLeaveRequestLine.UploadFilePath
            };
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            _logger.LogInformation($"Received Create leave request. Payload: {JsonConvert.SerializeObject(leaveRequestLineItem)} from remote address: {RemoteIpAddress}");
            var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();

            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest(new { responseMessage = $"Missing authorization header value", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });
            }
            var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });
            }
            var res = await _leaveRequestService.CreateLeaveRequestLineItem(leaveRequestLineItem);
            return Ok(res);
        }

        [Authorize]
        [HttpPost("CreateMultiple")]
        public async Task<ActionResult<List<LeaveRequestLineItem>>> CreateLeaveRequest([FromBody] List<CreateLeaveRequestLineItem> leaveRequests)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            _logger.LogInformation($"Received Create leave request. Payload: {JsonConvert.SerializeObject(leaveRequests)} from remote address: {RemoteIpAddress}");
            var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();

            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest(new { responseMessage = $"Missing authorization header value", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });
            }
            var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });
            }
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

     
        [HttpPost("Reschedule")]
        [Authorize]
        public async Task<IActionResult> RescheduleLeaveRequest([FromBody] LeaveRequestLineItem leaveRequestLineItem)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                _logger.LogInformation($"Received RescheduleLeave request. Payload: {JsonConvert.SerializeObject(leaveRequestLineItem)} from remote address: {RemoteIpAddress}");

                var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();

                var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });
                }
                var res = await _leaveRequestService.RescheduleLeaveRequest(leaveRequestLineItem);
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


        [HttpGet("{Id}")]
        [Authorize]
        public async Task<IActionResult> GetLeaveRequestLineItem(long Id)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                _logger.LogInformation($"Received GetLeave request. Payload: {JsonConvert.SerializeObject(new { Id })} from remote address: {RemoteIpAddress}");

                var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
                var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

                }

                return Ok(await _leaveRequestService.GetLeaveRequestLineItem(Id));
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

        [HttpGet("Info")]
        [Authorize]
        public async Task<IActionResult> GetEmpLeaveInfo([FromQuery] long CompanyId, [FromQuery] long EmployeeId,[FromQuery] string LeaveStatus = "Active")
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                _logger.LogInformation($"Received GetEmpLeaveInfo request. Payload: {JsonConvert.SerializeObject(new { CompanyId, EmployeeId, LeaveStatus })} from remote address: {RemoteIpAddress}");

                var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
                var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

                }
                var res = await _leaveRequestService.GetEmpLeaveInfo(EmployeeId, CompanyId, LeaveStatus);
                return Ok(new BaseResponse { Data = res, ResponseCode = "00", ResponseMessage = "Success"});
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
        [HttpGet("GetAllLeaveRequest")]
        public async Task<IActionResult> GetAllLeaveRequest([FromQuery] string CompanyID)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                _logger.LogInformation($"Received GetAllLeaveRequest. Payload: {JsonConvert.SerializeObject(new { CompanyID })} from remote address: {RemoteIpAddress}");

                var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
                var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

                }

                return Ok(await _leaveRequestService.GetAllLeaveRequest(CompanyID));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: Controller Method : GetAllLeaveRequest ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllLeaveRequest ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetEmpLeaveRequests")]
        public async Task<IActionResult> GetAllLeaveRequestLineItems([FromQuery] string CompanyID)
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

                return Ok(await _leaveRequestService.GetAllLeaveRquestLineItems(Convert.ToInt64(CompanyID)));
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

        [Authorize]
        [HttpGet("GetEmployeeLeaveRequests")]
        public async Task<IActionResult> GetEmployeeLeaveRequests([FromQuery] long CompanyID, long EmployeeId)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                _logger.LogInformation($"Received GetEmployeeLeaveRequests. Payload: {JsonConvert.SerializeObject(new { CompanyID, EmployeeId })} from remote address: {RemoteIpAddress}");

                var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
                var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

                }
                var leave = await _leaveRequestService.GetEmployeeLeaveRequests(CompanyID, EmployeeId);
                if (leave.Any())
                {
                    response.Data = leave;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Leave requests fetched successfully.";
                    return Ok(response);
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No Leave request found.";
                response.Data = null;
                return Ok(response);
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

        //[HttpGet("GetEmpLeaveRequestLineItems")]
        //[Authorize]
        //public async Task<IActionResult> GetEmpLeaveRequests([FromQuery] long CompanyId, [FromQuery] long EmployeeId, [FromQuery] string LeaveStatus)
        //{
        //    var response = new BaseResponse();
        //    try
        //    {
        //        var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //        _logger.LogInformation($"Received GetEmpLeaveInfo request. Payload: {JsonConvert.SerializeObject(new { CompanyId, EmployeeId, LeaveStatus })} from remote address: {RemoteIpAddress}");

        //        var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
        //        var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
        //        if (accessUser.data == null)
        //        {
        //            return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

        //        }
        //        var res = await _leaveRequestService.GetEmpLeaveRequests(EmployeeId, CompanyId, LeaveStatus);
        //        return Ok(new BaseResponse { Data = res, ResponseCode = "00", ResponseMessage = "Success" });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: ControllerMethod : GetLeaveRequestbyId ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured: ControllerMethod : GetLeaveRequestbyId ==> {ex.Message}";
        //        response.Data = null;
        //        return Ok(response);
        //    }

        //}
        
    }
}
