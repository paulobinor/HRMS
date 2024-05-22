using hrms_be_backend_business.ILogic;
using hrms_be_backend_business.Logic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace hrms_be_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _logger = logger;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(typeof(ExecutedResult<LoginResponse>), 200)]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {           
            var IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var Port = Request.HttpContext.Connection.RemotePort.ToString();
            var res = this.CustomResponse(await _authService.Login(login, IpAddress, Port));
            return res;           
        }

        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel refresh)
        {
            //use fv to validate RefreshTokenModel input
            var response = new RefreshTokenResponse();
            try
            {
                var IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var Port = Request.HttpContext.Connection.RemotePort.ToString();
                return Ok(await _authService.RefreshToken(refresh, IpAddress, Port));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : RefreshToken ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : RefreshToken ==> {ex.Message}";
                return Ok(response);
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(LogoutDto logout)
        {
            var response = new BaseResponse();
            try
            {
                var IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var Port = Request.HttpContext.Connection.RemotePort.ToString();
                return Ok(await _authService.Logout(logout, IpAddress, Port));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : Logout ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : Logout ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }
        [AllowAnonymous]
        [HttpPost("ChangeDefaultPassword")]
        [ProducesResponseType(typeof(ExecutedResult<LoginResponse>), 200)]
        public async Task<IActionResult> ChangeDefaultPassword([FromBody] ChangeDefaultPasswordDto payload)
        {
            var IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var Port = Request.HttpContext.Connection.RemotePort.ToString();
            return this.CustomResponse(await _authService.ChangeDefaultPassword(payload, IpAddress, Port));
        }
        [Authorize]
        [HttpPost("ChangePassword")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _authService.ChangePassword(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
    }
}
