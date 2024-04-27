﻿using hrms_be_backend_business.ILogic;
using hrms_be_backend_business.Logic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.Models;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Claims;
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
        public async Task<IActionResult> CreateLeaveRequest([FromBody] LeaveRequestLineItem leaveRequestLineItem)
        {
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
        public async Task<IActionResult> GetEmpLeaveInfo([FromQuery] long CompanyId, [FromQuery] long EmployeeId,[FromQuery] string LeaveStatus)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                _logger.LogInformation($"Received GetEmpLeaveInfo request. Payload: {JsonConvert.SerializeObject(new { CompanyId, EmployeeId, LeaveStatus }) } from remote address: {RemoteIpAddress}");
              
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

                return Ok(await _leaveRequestService.GetAllLeaveRquest(CompanyID));
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

        #region Depricated
        //[HttpPost("DisaproveLeaveRequest")]
        //[Authorize]
        //public async Task<IActionResult> DisaproveLeaveRequest([FromBody] LeaveRequestDisapproved payload)
        //{
        //    var response = new BaseResponse();
        //    var requester = new RequesterInfo
        //    {
        //        Username = this.User.Claims.ToList()[2].Value,
        //        UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
        //        RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
        //        IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
        //        Port = Request.HttpContext.Connection.RemotePort.ToString()
        //    };

        //    return Ok(await _leaveRequestService.DisaproveLeaveRequest(payload, requester));
        //}



        //[Authorize]
        //[HttpGet("GetLeaveRequestbyId")]
        //public async Task<IActionResult> GetLeaveRequestbyId(long LeaveRequestID)
        //{
        //    var response = new BaseResponse();
        //    try
        //    {
        //        var requester = new RequesterInfo
        //        {
        //            Username = this.User.Claims.ToList()[2].Value,
        //            UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
        //            RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
        //            IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
        //            Port = Request.HttpContext.Connection.RemotePort.ToString()
        //        };

        //        return Ok(await _leaveRequestService.GetLeaveRequsetById(LeaveRequestID, requester));
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

        //[Authorize]
        //[HttpGet("GetLeaveRequestbyUserId")]
        //public async Task<IActionResult> GetLeaveRequestbyUserId(long UserId, long CompanyId)
        //{
        //    var response = new BaseResponse();
        //    try
        //    {
        //        var requester = new RequesterInfo
        //        {
        //            Username = this.User.Claims.ToList()[2].Value,
        //            UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
        //            RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
        //            IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
        //            Port = Request.HttpContext.Connection.RemotePort.ToString()
        //        };

        //        return Ok(await _leaveRequestService.GetLeaveRequsetByUerId(UserId, CompanyId, requester));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: ControllerMethod : GetLeaveRequsetByUerId ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured: ControllerMethod : GetLeaveRequsetByUerId ==> {ex.Message}";
        //        response.Data = null;
        //        return Ok(response);
        //    }

        //}

        //[Authorize]
        //[HttpGet("GetLeaveRequestbyCompanyId")]
        //public async Task<IActionResult> GetLeaveRequestbyCompanyId(string RequestYear, long CompanyID)
        //{
        //    var response = new BaseResponse();
        //    try
        //    {
        //        var requester = new RequesterInfo
        //        {
        //            Username = this.User.Claims.ToList()[2].Value,
        //            UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
        //            RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
        //            IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
        //            Port = Request.HttpContext.Connection.RemotePort.ToString()
        //        };

        //        return Ok(await _leaveRequestService.GetLeaveRquestbyCompanyId(RequestYear, CompanyID, requester));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: ControllerMethod : GetLeaveTypebyCompanyId ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured: ControllerMethod : GetLeaveTypebyCompanyId ==> {ex.Message}";
        //        response.Data = null;
        //        return Ok(response);
        //    }

        //}
        //[Authorize]
        //[HttpGet("GetLeaveRequestPendingApproval")]
        //public async Task<IActionResult> GetLeaveRequestPendingApproval()
        //{
        //    var response = new BaseResponse();
        //    try
        //    {
        //        var requester = new RequesterInfo
        //        {
        //            Username = this.User.Claims.ToList()[2].Value,
        //            UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
        //            RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
        //            IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
        //            Port = Request.HttpContext.Connection.RemotePort.ToString()
        //        };

        //        return Ok(await _leaveRequestService.getl.GetLeaveRequestPendingApproval(requester));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: ControllerMethod : GetAllLeaveRequest ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured: ControllerMethod : GetLeaveRequestPendingApproval ==> {ex.Message}";
        //        response.Data = null;
        //        return Ok(response);
        //    }
        //}

        //[Authorize]
        //[HttpGet("GetLeaveRequestbyCompanyId")]
        //public async Task<IActionResult> GetLeaveRequestbyCompanyId(string RequestYear, long CompanyID)
        //{
        //    var response = new BaseResponse();
        //    try
        //    {
        //        var requester = new RequesterInfo
        //        {
        //            Username = this.User.Claims.ToList()[2].Value,
        //            UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
        //            RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
        //            IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
        //            Port = Request.HttpContext.Connection.RemotePort.ToString()
        //        };
        //    }

        //[Authorize]
        //[HttpPost("CreateLeaveRequestOld")]
        //public async Task<IActionResult> CreateLeaveRequestOld([FromBody] LeaveRequestCreate CreateDto)
        //{
        //    var response = new BaseResponse();
        //    var requester = new RequesterInfo
        //    {
        //        Username = this.User.Claims.ToList()[2].Value,
        //        UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
        //        RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
        //        IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
        //        Port = Request.HttpContext.Connection.RemotePort.ToString()
        //    };

        //    return Ok(await _leaveRequestService.CreateLeaveRequest(CreateDto, requester));
        //}
        //[HttpPost("RescheduleLeaveRequestOld")]
        //[Authorize]
        //public async Task<IActionResult> RescheduleLeaveRequestOld([FromBody] RescheduleLeaveRequest updateDto)
        //{
        //    var response = new BaseResponse();
        //    try
        //    {
        //        var requester = new RequesterInfo
        //        {
        //            Username = this.User.Claims.ToList()[2].Value,
        //            UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
        //            RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
        //            IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
        //            Port = Request.HttpContext.Connection.RemotePort.ToString()
        //        };
        //        var res = await _leaveRequestService.RescheduleLeaveRequest(updateDto, requester);
        //        return Ok(res);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: ControllerMethod : RescheduleLeaveRequest ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured: ControllerMethod : RescheduleLeaveRequest ==> {ex.Message}";
        //        response.Data = null;
        //        return Ok(response);
        //    }
        //}

        //[HttpPost("ApproveLeaveRequestOld")]
        //[Authorize]
        //public async Task<IActionResult> ApproveLeaveRequestOld(long LeaveRequestID)
        //{
        //    var response = new BaseResponse();
        //    var requester = new RequesterInfo
        //    {
        //        Username = this.User.Claims.ToList()[2].Value,
        //        UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
        //        RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
        //        IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
        //        Port = Request.HttpContext.Connection.RemotePort.ToString()
        //    };

        //    return Ok(await _leaveRequestService.ApproveLeaveRequest(LeaveRequestID, requester));
        //} 
        #endregion
    }
}
