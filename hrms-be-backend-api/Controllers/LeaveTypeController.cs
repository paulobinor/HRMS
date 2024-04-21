using hrms_be_backend_business.ILogic;
using hrms_be_backend_business.Logic;
using hrms_be_backend_common.Models;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace hrms_be_backend_api.LeaveModuleController.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypeController : ControllerBase
    {
        private readonly ILogger<LeaveTypeController> _logger;
        private readonly ILeaveTypeService _LeaveTypeService;
        private readonly IAuthService _authService;

        public LeaveTypeController(ILogger<LeaveTypeController> logger, ILeaveTypeService LeaveTypeService, IAuthService authService)
        {
            _logger = logger;
            _LeaveTypeService = LeaveTypeService;
            _authService = authService;
        }


        [HttpPost("CreateLeaveType")]
        [Authorize]
        public async Task<IActionResult> CreateLeaveType([FromBody] CreateLeaveTypeDTO CreateDto)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
                accessToken = accessToken.ToString().Replace("bearer", "").Trim();

                var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return Unauthorized( new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString()});

                }

                return Ok(await _LeaveTypeService.CreateLeaveType(CreateDto, accessUser.data.OfficialMail));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : CreateLeaveType ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : CreateLeaveType ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("UpdateLeaveType")]
        [Authorize]
        public async Task<IActionResult> UpdateLeaveType([FromBody] UpdateLeaveTypeDTO updateDto)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
                var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });
                }

                return Ok(await _LeaveTypeService.UpdateLeaveType(updateDto, accessUser.data.OfficialMail));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : UpdateLeaveType ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : UpdateLeaveType ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("DeleteLeaveType")]
        [Authorize]
        public async Task<IActionResult> DeleteLeaveType([FromBody] DeleteLeaveTypeDTO deleteDto)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
                var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });
                }

                return Ok(await _LeaveTypeService.DeleteLeaveType(deleteDto, accessUser.data.OfficialMail));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DeleteLeaveType ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DeleteLeaveType ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetAllActiveLeaveType")]
        public async Task<IActionResult> GetAllActiveLeaveType()
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
                var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });
                }

                return Ok(await _LeaveTypeService.GetAllActiveLeaveType());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllActiveLeaveType ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllActiveLeaveType ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [Authorize]
        [HttpGet("GetAllLeaveType")]
        public async Task<IActionResult> GetAllLeaveType()
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();

                var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

                }

                return Ok(await _LeaveTypeService.GetAllLeaveType());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllLeaveType ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllLeaveType ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetLeaveTypebyId")]
        public async Task<IActionResult> GetLeaveTypebyId(long LeaveTypeId)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();

                var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

                }

                return Ok(await _LeaveTypeService.GetLeaveTypeById(LeaveTypeId));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetLeaveTypebyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetLeaveTypebyId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [Authorize]
        [HttpGet("GetLeaveTypebyCompanyId")]
        public async Task<IActionResult> GetLeaveTypebyCompanyId(long CompanyID, string RequestId)
        {
            if (string.IsNullOrEmpty(RequestId))
            {
                RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");  
            }
            _logger.LogInformation($"{RequestId} - Received request to get leave type for CompanyId: {CompanyID}");
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();

                var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

                }

                return Ok(await _LeaveTypeService.GetLeavebyCompanyId(CompanyID));
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

    }
}
