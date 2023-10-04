using hrms_be_backend_business.ILogic;
using hrms_be_backend_business.Logic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hrms_be_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAppModulePrivilegeController : ControllerBase
    {
        private readonly ILogger<UserAppModulePrivilegeController> _logger;
        private readonly IUserAppModulePrivilegeService _userAppModulePrivilegeService;

        public UserAppModulePrivilegeController(ILogger<UserAppModulePrivilegeController> logger, IUserAppModulePrivilegeService userAppModulePrivilegeService)
        {
            _logger = logger;
            _userAppModulePrivilegeService = userAppModulePrivilegeService;
        }

        [HttpGet("GetUserAppModulePrivileges")]
        public async Task<IActionResult> GetUserAppModulePrivileges()
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _userAppModulePrivilegeService.GetUserAppModulePrivileges(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetUserAppModulePrivileges ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet("GetUserAppModulePrivilegesByUserID")]
        public async Task<IActionResult> GetUserAppModulePrivilegesByUserID(long userID)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _userAppModulePrivilegeService.GetUserAppModulePrivilegesByUserID(userID,requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetUserAppModulePrivilegesByUserID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet("GetAppModulePrivileges")]
        public async Task<IActionResult> GetAppModulePrivileges()
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _userAppModulePrivilegeService.GetAppModulePrivileges( requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetUserAppModulePrivilegesByUserID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet("GetAppModulePrivilegesByModuleID/{appModuleID}")]
        public async Task<IActionResult> GetAppModulePrivilegesByModuleID(long appModuleID)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _userAppModulePrivilegeService.GetAppModulePrivilegesByAppModuleID(appModuleID,requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAppModulePrivilegesByModuleID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }
        [HttpGet("GetPendingUserAppModulePrivileges")]
        public async Task<IActionResult> GetPendingUserAppModulePrivileges()
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _userAppModulePrivilegeService.GetPendingUserAppModulePRivilege( requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetPendingUserAppModulePrivileges ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("CreateUserAppModulePrivileges")]
        public async Task<IActionResult> CreateUserAppModulePrivileges(CreateUserAppModulePrivilegesDTO request)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _userAppModulePrivilegeService.CreateUserAppModulePrivileges(request ,requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : CreateUserAppModulePrivileges ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet("ApproveUserAppModulePrivileges/{userAppModulePrivilegeID}")]
        public async Task<IActionResult> ApproveUserAppModulePrivileges(long userAppModulePrivilegeID)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _userAppModulePrivilegeService.ApproveUserAppModulePrivilege(userAppModulePrivilegeID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : ApproveUserAppModulePrivileges ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet("DisapproveUserAppModulePrivileges/{userAppModulePrivilegeID}")]
        public async Task<IActionResult> DisapproveUserAppModulePrivileges(long userAppModulePrivilegeID)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _userAppModulePrivilegeService.DisapproveUserAppModulePrivilage(userAppModulePrivilegeID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DisapproveUserAppModulePrivileges ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet("UserAppModulePrivilegesActivationSwitch/{userAppModulePrivilegeID}")]
        public async Task<IActionResult> UserAppModulePrivilegesActivationSwitch(long userAppModulePrivilegeID)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _userAppModulePrivilegeService.UserAppModulePrivilegeActivationSwitch(userAppModulePrivilegeID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : UserAppModulePrivilegesActivationSwitch ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet("DeleteUserAppModulePrivileges/{userAppModulePrivilegeID}")]
        public async Task<IActionResult> DeleteUserAppModulePrivileges(long userAppModulePrivilegeID)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _userAppModulePrivilegeService.DeleteUserAppModulePrivilege(userAppModulePrivilegeID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DeleteUserAppModulePrivileges ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }
    }
}
