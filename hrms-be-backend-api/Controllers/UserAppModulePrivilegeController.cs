using hrms_be_backend_business.ILogic;
using hrms_be_backend_business.Logic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace hrms_be_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserAppModulePrivilegeController : BaseController
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
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
                accessToken = accessToken.ToString().Replace("bearer", "").Trim();

                return this.CustomResponse(await _userAppModulePrivilegeService.GetUserAppModulePrivileges(accessToken, claim, RemoteIpAddress, RemotePort));
           
        }

        [HttpGet("GetUserAppModulePrivilegesByUserID")]
        public async Task<IActionResult> GetUserAppModulePrivilegesByUserID(long userID)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _userAppModulePrivilegeService.GetUserAppModulePrivilegesByUserID(userID, accessToken, claim, RemoteIpAddress, RemotePort));
           
        }

        [HttpGet("GetAppModulePrivileges")]
        public async Task<IActionResult> GetAppModulePrivileges()
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _userAppModulePrivilegeService.GetAppModulePrivileges(accessToken, claim, RemoteIpAddress, RemotePort));
            
        }

        [HttpGet("GetAppModulePrivilegesByModuleID/{appModuleID}")]
        public async Task<IActionResult> GetAppModulePrivilegesByModuleID(long appModuleID)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _userAppModulePrivilegeService.GetAppModulePrivilegesByAppModuleID(appModuleID, accessToken, claim, RemoteIpAddress, RemotePort));
           
        }
        [HttpGet("GetPendingUserAppModulePrivileges")]
        public async Task<IActionResult> GetPendingUserAppModulePrivileges()
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _userAppModulePrivilegeService.GetPendingUserAppModulePRivilege(accessToken, claim, RemoteIpAddress, RemotePort));
           
        }

        [HttpPost("CreateUserAppModulePrivileges")]
        public async Task<IActionResult> CreateUserAppModulePrivileges(CreateUserAppModulePrivilegesDTO request)
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _userAppModulePrivilegeService.CreateUserAppModulePrivileges(request , accessToken, claim, RemoteIpAddress, RemotePort));
           
        }

        [HttpGet("ApproveUserAppModulePrivileges/{userAppModulePrivilegeID}")]
        public async Task<IActionResult> ApproveUserAppModulePrivileges(long userAppModulePrivilegeID)
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _userAppModulePrivilegeService.ApproveUserAppModulePrivilege(userAppModulePrivilegeID, accessToken, claim, RemoteIpAddress, RemotePort));
           
        }

        [HttpGet("DisapproveUserAppModulePrivileges/{userAppModulePrivilegeID}")]
        public async Task<IActionResult> DisapproveUserAppModulePrivileges(long userAppModulePrivilegeID)
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _userAppModulePrivilegeService.DisapproveUserAppModulePrivilage(userAppModulePrivilegeID, accessToken, claim, RemoteIpAddress, RemotePort));
           
        }

        [HttpGet("UserAppModulePrivilegesActivationSwitch/{userAppModulePrivilegeID}")]
        public async Task<IActionResult> UserAppModulePrivilegesActivationSwitch(long userAppModulePrivilegeID)
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _userAppModulePrivilegeService.UserAppModulePrivilegeActivationSwitch(userAppModulePrivilegeID, accessToken, claim, RemoteIpAddress, RemotePort));
            
        }

        [HttpGet("DeleteUserAppModulePrivileges/{userAppModulePrivilegeID}")]
        public async Task<IActionResult> DeleteUserAppModulePrivileges(long userAppModulePrivilegeID)
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _userAppModulePrivilegeService.DeleteUserAppModulePrivilege(userAppModulePrivilegeID, accessToken, claim, RemoteIpAddress, RemotePort));
           
        }
    }
}
