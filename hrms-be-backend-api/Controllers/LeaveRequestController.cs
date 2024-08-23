﻿using hrms_be_backend_business.ILogic;
using hrms_be_backend_business.Logic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
using hrms_be_backend_common.Models;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using iText.Kernel.Geom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Collections.Generic;
using System.Security.Claims;
using static iText.Signatures.LtvVerification;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace hrms_be_backend_api.LeaveModuleController.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    //  [Authorize]
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

        /// <summary>
        /// Endpoint to create leave request for an employee within given company
        /// </summary>
        /// <param name="createLeaveRequestLine">Request Parameters</param>
        /// <returns>BaseResponse</returns>
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
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
            //var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            //_logger.LogInformation($"Received Create leave request. Payload: {JsonConvert.SerializeObject(leaveRequestLineItem)} from remote address: {RemoteIpAddress}");
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
            var res = await _leaveRequestService.CreateLeaveRequestLineItem(leaveRequestLineItem);
            return Ok(res);
        }


        /// <summary>
        /// Endpoint to reschedule an existing pending or approved leave request for an employee within given company within a given period
        /// </summary>
        /// <param name="leaveRequestLineItem">Request Parameters</param>
        /// <returns>BaseResponse</returns>
        [HttpPost("Reschedule")]
        public async Task<IActionResult> RescheduleLeaveRequest([FromBody] LeaveRequestLineItem leaveRequestLineItem)
        {
            var response = new BaseResponse();
            try
            {
                //var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                //_logger.LogInformation($"Received RescheduleLeave request. Payload: {JsonConvert.SerializeObject(leaveRequestLineItem)} from remote address: {RemoteIpAddress}");

                //var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();

                //var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                //if (accessUser.data == null)
                //{
                //    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });
                //}
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
                var res = await _leaveRequestService.GetLeaveRequestLineItem(Id);
                return Ok(res);
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

        /// <summary>
        /// Gets leave info for a single Employee within a given Company within a given period
        /// </summary>
        /// <param name="CompanyId">Request Parameters</param>
        /// <param name="EmployeeId">Request Parameters</param>
        /// <param name="year">Request Parameters</param>
        /// <returns>BaseResponse</returns>
        [HttpGet("Info")]
        public async Task<IActionResult> GetEmpLeaveInfo([FromQuery] long CompanyId, [FromQuery] long EmployeeId,[FromQuery] string year = null)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                _logger.LogInformation($"Received GetEmpLeaveInfo request. Payload: {JsonConvert.SerializeObject(new { CompanyId, EmployeeId, year })} from remote address: {RemoteIpAddress}");

                //var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
                //var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                //if (accessUser.data == null)
                //{
                //    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

                //}
                if (string.IsNullOrEmpty(year)) year = DateTime.Now.Year.ToString(); 
                var res = await _leaveRequestService.GetEmpLeaveInfo(EmployeeId, CompanyId, year);
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


        /// <summary>
        /// Endpoint to get a list of pending/past leave request for a given company
        /// </summary>
        /// <param name="CompanyID">Request Parameters</param>
        /// <returns>BaseResponse</returns>
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("GetAllLeaveRequest")]
        public async Task<IActionResult> GetAllLeaveRequest([FromQuery] string CompanyID)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                _logger.LogInformation($"Received GetAllLeaveRequest. Payload: {JsonConvert.SerializeObject(new { CompanyID })} from remote address: {RemoteIpAddress}");

                //var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
                //var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                //if (accessUser.data == null)
                //{
                //    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

                //}

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

       
        /// <summary>
        /// Endpoint to get a paged list of pending/past leave request for a given company
        /// </summary>
        /// <param name="CompanyID">Request Parameters</param>
        /// <returns>BaseResponse</returns>
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("GetAllPagedLeaveRequest")]
        public async Task<IActionResult> GetAllPagedLeaveRequest([FromQuery] string CompanyID, DateTime? startDate, DateTime? endDate, int pageNumber = 1, int pageSize = 10)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                _logger.LogInformation($"Received GetAllLeaveRequest. Payload: {JsonConvert.SerializeObject(new { CompanyID })} from remote address: {RemoteIpAddress}");

                //var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
                //var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                //if (accessUser.data == null)
                //{
                //    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

                //}
                if (startDate == null)
                {
                    startDate = new DateTime(DateTime.Now.Year, 1, 1);
                }
                else
                {
                    startDate = startDate.Value.AddDays(-1);
                }
                if (endDate == null)
                {
                    endDate = new DateTime(DateTime.Now.Year, 12, 31);
                }
                else
                {
                    endDate = endDate.Value.AddDays(1);
                }
                List<EmpLeaveRequestInfo> finalRes = new List<EmpLeaveRequestInfo>();
                var res = await _leaveRequestService.GetAllLeaveRequest(CompanyID);
                var requests = (List<EmpLeaveRequestInfo>)res.Data;

               // requests = requests.FindAll(x => x.DateCreated >= startDate && x.DateCreated <= endDate);

                foreach (var request in requests)
                {
                    request.leaveRequestLineItems  = request.leaveRequestLineItems.FindAll(x => x.startDate >= startDate && x.endDate <= endDate).ToList();
                }
                foreach (var request in requests)
                {
                    if (request.leaveRequestLineItems.Count > 0)
                    {
                        finalRes.Add(request); //.leaveRequestLineItems
                    }
                }
               
                //var pagedRes = hrms_be_backend_business.Helpers.Utilities.GetPagedList(finalRes, pageNumber, pageSize);

                int totalItems = 0; int totalPages = 0;
                List<EmpLeaveRequestInfo> items = null;
                if (finalRes != null && finalRes.Count > 0)
                {
                    totalItems = finalRes.Count;
                    totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                    items = finalRes
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                }
                else
                {
                    totalItems = 0;
                    totalPages = 1;
                }

                var pagedRes = new
                {
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    Items = items
                };
                res.Data = pagedRes;
                return Ok(res);
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


        /// <summary>
        /// Gets leave info for a single Employee within a given Company
        /// </summary>
        /// <param name="CompanyId">Request Parameters</param>
        /// <param name="EmployeeId">Request Parameters</param>
        /// <returns>BaseResponse</returns>
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("GetEmployeeLeaveRequests")]
        public async Task<IActionResult> GetEmployeeLeaveRequests([FromQuery] long CompanyID, long EmployeeId)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                _logger.LogInformation($"Received GetEmployeeLeaveRequests. Payload: {JsonConvert.SerializeObject(new { CompanyID, EmployeeId })} from remote address: {RemoteIpAddress}");

             //   var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
                //var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                //if (accessUser.data == null)
                //{
                //    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

                //}
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

        

        #region Depricated
        ///// <summary>
        ///// Endpoint to get a list of pending/past leave request for a given company within a given period
        ///// </summary>
        ///// <param name="CompanyID">Request Parameters</param>
        ///// <returns>BaseResponse</returns>
        //[ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        //[HttpGet("GetEmpLeaveRequests")]
        //public async Task<IActionResult> GetAllLeaveRequestLineItems([FromQuery] string CompanyID)
        //{
        //    var response = new BaseResponse();
        //    try
        //    {
        //        var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //        _logger.LogInformation($"Received GetAllLeaveRequestLineItems. Payload: {JsonConvert.SerializeObject(new { CompanyID })} from remote address: {RemoteIpAddress}");

        //        //var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
        //        //var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
        //        //if (accessUser.data == null)
        //        //{
        //        //    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

        //        //}

        //        return Ok(await _leaveRequestService.GetAllLeaveRquestLineItems(Convert.ToInt64(CompanyID)));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: Controller Method : GetAllLeaveRequestLineItems ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllLeaveRequest ==> {ex.Message}";
        //        response.Data = null;
        //        return Ok(response);
        //    }
        //}

        ///// <summary>
        ///// Endpoint to get a paged list of pending/past leave request for a given company within a given period
        ///// </summary>
        ///// <param name="CompanyID">Request Parameters</param>
        ///// <param name="startdate">Request Parameters</param>
        ///// <param name="endDate">Request Parameters</param>
        ///// <param name="pageNumber">Request Parameters</param>
        ///// <param name="pageSize">Request Parameters</param>
        ///// <returns>BaseResponse</returns>
        //[ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        //[HttpGet("GetPagedEmpLeaveRequests")]
        //public async Task<IActionResult> GetAllPagedLeaveRequestLineItems([FromQuery] string CompanyID, DateTime? startDate, DateTime? endDate, int pageNumber = 1, int pageSize = 10)
        //{
        //    var response = new BaseResponse();
        //    try
        //    {
        //        var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //        _logger.LogInformation($"Received GetAllLeaveRequestLineItems. Payload: {JsonConvert.SerializeObject(new { CompanyID })} from remote address: {RemoteIpAddress}");

        //        //var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
        //        //var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
        //        //if (accessUser.data == null)
        //        //{
        //        //    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

        //        //}
        //        if (startDate == null)
        //        {
        //            startDate = new DateTime(DateTime.Now.Year, 1, 1);
        //        }
        //        if (endDate == null)
        //        {
        //            endDate = new DateTime(DateTime.Now.Year, 12, 31);
        //        }

        //        var res = await _leaveRequestService.GetAllLeaveRquestLineItems(Convert.ToInt64(CompanyID));
        //        var requests = (List<LeaveRequestLineItemDto>)res.Data;

        //        requests = requests.FindAll(x => x.startDate >= startDate && x.endDate <= endDate);
        //        int totalItems = 0; int totalPages = 0;
        //        List<LeaveRequestLineItemDto> items = null;
        //        if (requests != null && requests.Count > 0)
        //        {
        //            totalItems = requests.Count;
        //            totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        //            items = requests
        //                .Skip((pageNumber - 1) * pageSize)
        //                .Take(pageSize)
        //                .ToList();
        //        }
        //        else
        //        {
        //            totalItems = 0;
        //            totalPages = 1;
        //        }

        //        var pagedRes = new
        //        {
        //            TotalItems = totalItems,
        //            PageNumber = pageNumber,
        //            PageSize = pageSize,
        //            TotalPages = totalPages,
        //            Items = items
        //        };
        //        res.Data = pagedRes;
        //        return Ok(res);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: Controller Method : GetAllLeaveRequestLineItems ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllLeaveRequest ==> {ex.Message}";
        //        response.Data = null;
        //        return Ok(response);
        //    }
        //} 
        #endregion
    }
}
