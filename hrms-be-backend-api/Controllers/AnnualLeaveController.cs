using hrms_be_backend_business.Helpers;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_business.Logic;
using hrms_be_backend_common.DTO;
using hrms_be_backend_common.Models;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
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

        /// <summary>
        /// Endpoint to create annual leave request for an employee within given company
        /// </summary>
        /// <param name="leaveRequests">Request Parameters</param>
        /// <returns>BaseResponse</returns>
        [HttpPost("CreateMultiple")]
        public async Task<ActionResult<List<LeaveRequestLineItem>>> CreateLeaveRequest([FromBody] List<CreateLeaveRequestLineItem> leaveRequests)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            _logger.LogInformation($"Received Create leave request. Payload: {JsonConvert.SerializeObject(leaveRequests)} from remote address: {RemoteIpAddress}");
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

        /// <summary>
        /// Endpoint to reschedule an existing pending or approved annual leave request for an employee within given company within a given period
        /// </summary>
        /// <param name="leaveRequestLineItems">Request Parameters</param>
        /// <returns>BaseResponse</returns>
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost("RescheduleAnnual")]
        public async Task<IActionResult> Reschedule([FromBody] List<LeaveRequestLineItem> leaveRequestLineItems)
        {
            var response = new BaseResponse();
            try
            {
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


        /// <summary>
        /// Endpoint to get a list of pending/past annual leave request for a given company within a given period
        /// </summary>
        /// <param name="CompanyID">Request Parameters</param>
        /// <param name="year">Request Parameters</param>
        /// <returns>BaseResponse</returns>
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("GetAnnualLeaveRequests")]
        public async Task<IActionResult> GetAnnualLeaveRequests([FromQuery] string CompanyID, string year = null)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                _logger.LogInformation($"Received GetAllLeaveRequestLineItems. Payload: {JsonConvert.SerializeObject(new { CompanyID })} from remote address: {RemoteIpAddress}");

                //   var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
                //   var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                //if (accessUser.data == null)
                //{
                //    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

                //}
                if (string.IsNullOrEmpty(year))
                {
                    year = DateTime.Now.ToString("yyyy");
                }
                var res = await _leaveRequestService.GetAnnualLeaveRequests(Convert.ToInt64(CompanyID), year);
                //var requests = (List<LeaveRequestLineItemDto>)res.Data;
                //var totalItems = requests.Count;
                //var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                //var items = requests
                //    .Skip((pageNumber - 1) * pageSize)
                //    .Take(pageSize)
                //    .ToList();

                //var pagedRes = new
                //{
                //    TotalItems = totalItems,
                //    PageNumber = pageNumber,
                //    PageSize = pageSize,
                //    TotalPages = totalPages,
                //    Items = items
                //};
                //res.Data = pagedRes;
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

       
        /// <summary>
        /// Endpoint to get a paged list of pending/past annual leave request for a given company within a given period
        /// </summary>
        /// <param name="CompanyID">Request Parameters</param>
        /// <param name="startdate">Request Parameters</param>
        /// <param name="endDate">Request Parameters</param>
        /// <param name="pageNumber">Request Parameters</param>
        /// <param name="pageSize">Request Parameters</param>
        /// <returns>BaseResponse</returns>
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("GetPagedAnnualLeaveRequests")]
        public async Task<IActionResult> GetPagedAnnualLeaveRequests([FromQuery] string CompanyID, DateTime? startdate, DateTime? endDate, string ApprovalPosition = "All", string approvalStatus = "All", int pageNumber = 1, int pageSize = 10, string year = null)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                _logger.LogInformation($"Received GetAllLeaveRequestLineItems. Payload: {JsonConvert.SerializeObject(new { CompanyID })} from remote address: {RemoteIpAddress}");

                //   var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
                //   var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                //if (accessUser.data == null)
                //{
                //    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

                //}
                if (startdate == null)
                {
                    startdate = new DateTime(DateTime.Now.Year, 1, 1);
                }
                else
                {
                    startdate = startdate.Value.AddDays(-1);
                }
                if (endDate == null)
                {
                    endDate = new DateTime(DateTime.Now.Year, 12, 31);
                }
                else
                {
                    endDate = endDate.Value.AddDays(1);
                }
                if (string.IsNullOrEmpty(year))
                {
                    year = DateTime.Now.ToString("yyyy");
                }
                var res = await _leaveRequestService.GetAnnualLeaveRequests(Convert.ToInt64(CompanyID), year);

                List<AnnualLeaveDto> finalRes = new List<AnnualLeaveDto>();
                var requests = (List<AnnualLeaveDto>)res.Data;
                if (requests == null)
                {
                    return Ok(res);
                }

                foreach (var request in requests)
                {
                    request.leaveRequestLineItems = request.leaveRequestLineItems.FindAll(x => x.startDate >= startdate && x.endDate <= endDate).ToList();
                }
                foreach (var request in requests)
                {
                    if (request.leaveRequestLineItems.Count > 0)
                    {
                        finalRes.Add(request); //.leaveRequestLineItems
                    }
                }
                PagedListModel<AnnualLeaveDto> pagedRes = null;
                if (!string.IsNullOrEmpty(approvalStatus))
                {
                    if (!approvalStatus.Equals("All", StringComparison.OrdinalIgnoreCase))
                    {
                        finalRes = finalRes.FindAll(x => x.ApprovalStatus == approvalStatus);
                    }
                }
                if (!string.IsNullOrEmpty(ApprovalPosition))
                {
                    if (!ApprovalPosition.Equals("All", StringComparison.OrdinalIgnoreCase))
                    {
                        finalRes = finalRes.FindAll(x => x.ApprovalPosition == ApprovalPosition);
                    }
                }
                pagedRes = hrms_be_backend_business.Helpers.Utilities.GetPagedList(finalRes, pageNumber, pageSize);
                //  requests = requests.FindAll(x => x.DateCreated >= startdate && x.DateCreated <= endDate);
                //int totalItems = 0; int totalPages = 0;
                //List<AnnualLeaveDto> items = null;
                //if (finalRes != null && finalRes.Count > 0)
                //{
                //    totalItems = finalRes.Count;
                //    totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                //    items = finalRes
                //        .Skip((pageNumber - 1) * pageSize)
                //        .Take(pageSize)
                //        .ToList();
                //}
                //else
                //{
                //    totalItems = 0;
                //    totalPages = 1;
                //}

                //var pagedRes = new
                //{
                //    TotalItems = totalItems,
                //    PageNumber = pageNumber,
                //    PageSize = pageSize,
                //    TotalPages = totalPages,
                //    Items = items
                //};
                res.Data = pagedRes;
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

        /// <summary>
        /// Gets Annual leave info for a single Employee within a given Company
        /// </summary>
        /// <param name="CompanyId">Request Parameters</param>
        /// <param name="EmployeeId">Request Parameters</param>
        /// <returns>BaseResponse</returns>
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Endpoint to approve or disapprove annual leave for a selected Employee
        /// </summary>
        /// <param name="leaveApprovalLineItem">Request Parameters</param>
        /// <returns>BaseResponse</returns>
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost("UpdateAnnualLeave")]
        public async Task<IActionResult> UpdateAnnualLeave(LeaveApprovalLineItem leaveApprovalLineItem)
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

        /// <summary>
        /// Endpoint to get list ofpending/past annual leave request assigned to ApprovalEmployeeId for approval or disapproval
        /// </summary>
        /// <param name="ApprovalEmployeeID">Request Parameters</param>
        /// <returns>BaseResponse</returns>
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("GetAnnualLeaveApprovals")]
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
