using ExcelDataReader.Log.Logger;
using hrms_be_backend_api.Dtos;
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
using System.Security.Claims;

namespace hrms_be_backend_api.LeaveModuleController.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeLeaveController : ControllerBase
    {
        private readonly ILogger<GradeLeaveController> _logger;
        private readonly IGradeLeaveService _GradeLeaveService;
        private readonly IAuthService _authService;

        public GradeLeaveController(ILogger<GradeLeaveController> logger, IGradeLeaveService GradeLeaveService, IAuthService authService)
        {
            _logger = logger;
            _GradeLeaveService = GradeLeaveService;
            _authService = authService;
        }

        [HttpPost("CreateGradeLeave")]
        [Authorize]
        public async Task<IActionResult> CreateGradeLeave([FromBody] CreateGradeLeaveDTO CreateDto)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                _logger.LogInformation($"Received Create grade leave request. Payload: {JsonConvert.SerializeObject(CreateDto)} from remote address: {RemoteIpAddress}");
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

                return Ok(await _GradeLeaveService.CreateGradeLeave(CreateDto, accessToken, RemoteIpAddress));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : CreateGradeLeave ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : CreateGradeLeave ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        //[HttpPost("CreateGradeLevelBulk")]
        //[ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        //public async Task<IActionResult> CreateGradeLevelBulk(IFormFile payload)
        //{
        //    var requester = new RequesterInfo
        //    {
        //        IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
        //        Port = Request.HttpContext.Connection.RemotePort.ToString()
        //    };
        //    var identity = HttpContext.User.Identity as ClaimsIdentity;
        //    IEnumerable<Claim> claim = identity.Claims;
        //    var accessToken = Request.Headers["Authorization"];
        //    accessToken = accessToken.ToString().Replace("bearer", "").Trim();
        //    return this.CustomResponse(await _gradeService.CreateGradeBulkUpload(payload, accessToken, claim, requester));
        //}


        [HttpPost("UpdateGradeLeave")]
        [Authorize]
        public async Task<IActionResult> UpdateGradeLeave([FromBody] UpdateGradeLeaveDTO updateDto)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                _logger.LogInformation($"Received UpdateGradeLeave request. Payload: {JsonConvert.SerializeObject(updateDto)} from remote address: {RemoteIpAddress}");
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
                updateDto.UserId = accessUser.data.UserId;
                return Ok(await _GradeLeaveService.UpdateGradeLeave(updateDto));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : UpdateGradeLeave ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : UpdateGradeLeave ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("DeleteGradeLeave")]
        [Authorize]
        public async Task<IActionResult> DeleteGradeLeave([FromBody] DeleteGradeLeaveDTO deleteDto)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                _logger.LogInformation($"Received DeleteGradeLeave request. Payload: {JsonConvert.SerializeObject(deleteDto)} from remote address: {RemoteIpAddress}");
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
                deleteDto.UserID = accessUser.data.UserId;

                return Ok(await _GradeLeaveService.DeleteGradeLeave(deleteDto));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DeleteGradeLeave ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DeleteGradeLeave ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetAllActiveGradeLeave")]
        public async Task<IActionResult> GetAllActiveGradeLeave()
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                _logger.LogInformation($"Received GetAllActiveGradeLeave request from remote address: {RemoteIpAddress}");
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

                return Ok(await _GradeLeaveService.GetAllActiveGradeLeave(new RequesterInfo()));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllActiveGradeLeave ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllActiveGradeLeave ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [Authorize]
        [HttpGet("GetAllGradeLeave")]
        public async Task<IActionResult> GetAllGradeLeave()
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                _logger.LogInformation($"Received get all leave request. Payload: {JsonConvert.SerializeObject("")} from remote address: {RemoteIpAddress}");
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

                return Ok(await _GradeLeaveService.GetAllGradeLeave(accessToken,RemoteIpAddress));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllGradeLeave ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllGradeLeave ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetGradeLeavebyId")]
        public async Task<IActionResult> GetGradeLeavebyId(long GradeLeaveID)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                _logger.LogInformation($"Received get all leave request. Payload: {JsonConvert.SerializeObject("")} from remote address: {RemoteIpAddress}");
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

                return Ok(await _GradeLeaveService.GetGradeLeaveById(GradeLeaveID, new RequesterInfo()));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetGradeLeavebyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetGradeLeavebyId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [Authorize]
        [HttpGet("GetGradeLeavebyCompanyId")]
        public async Task<IActionResult> GetGradeLeavebyCompanyId(long CompanyID)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                _logger.LogInformation($"Received get grade leave by company id request. Payload: {JsonConvert.SerializeObject(CompanyID)} from remote address: {RemoteIpAddress}");
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

                return Ok(await _GradeLeaveService.GetGradeLeavebyCompanyId(CompanyID, accessToken,RemoteIpAddress));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetGradeLeavebyCompanyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetGradeLeavebyCompanyId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [Authorize]
        [HttpGet("GetEmployeeGradeLeaveTypes")]
        public async Task<IActionResult> GetEmployeeGradeLeaveTypes([FromQuery] long CompanyID, [FromQuery] long EmployeeID)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                _logger.LogInformation($"Received get grade leave by grade id request. Payload: {JsonConvert.SerializeObject(CompanyID)} from remote address: {RemoteIpAddress}");
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

                var res = await _GradeLeaveService.GetEmployeeGradeLeaveTypes(CompanyID, EmployeeID);
                List<EmployeeGradeLeaveDto> empGradeLeaveList = new List<EmployeeGradeLeaveDto>();
                foreach (var item in res)
                {
                    var empgradeLeave = new EmployeeGradeLeaveDto
                    {
                        CompanyID = item.CompanyID,
                        GradeID = item.GradeID,
                        GenderID = item.GenderID,
                        GradeLeaveID = item.GradeLeaveID,
                        GradeName = item.GradeName,
                        LeaveTypeId = item.LeaveTypeId,
                        LeaveTypeName = item.LeaveTypeName,
                        MaximumNumberOfLeaveDays = item.MaximumNumberOfLeaveDays,
                        NumberOfVacationSplit = item.NumberOfVacationSplit,
                        NumbersOfDays = item.NumbersOfDays
                    };
                    empGradeLeaveList.Add(empgradeLeave);
                }
                response.Data = empGradeLeaveList;
                response.ResponseCode = "00";
                response.ResponseMessage = "GetEmployeeGradeLeaveTypes fetched Successfully";
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetGradeLeavebyCompanyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetGradeLeavebyCompanyId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }
    }
}
