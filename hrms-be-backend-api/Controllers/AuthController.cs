using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hrms_be_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
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
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var response = new LoginResponse();
            var IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var Port = Request.HttpContext.Connection.RemotePort.ToString();

            return Ok(await _authService.Login(login, IpAddress, Port));
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

        [HttpPost("SendEmailForPasswordChange")]
        [AllowAnonymous]
        [Authorize]
        public async Task<IActionResult> SendEmailForPasswordChange(RequestPasswordChange request)
        {
            var response = new BaseResponse();
            try
            {
                var IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var Port = Request.HttpContext.Connection.RemotePort.ToString();
                return Ok(await _authService.SendEmailForPasswordChange(request, IpAddress, Port));

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : SendEmailForPasswordChange ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : SendEmailForPasswordChange ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }


        }

        [HttpPost("ChangePassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePassword)
        {
            var response = new BaseResponse();

            try
            {
                var IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var Port = Request.HttpContext.Connection.RemotePort.ToString();
                return Ok(await _authService.ChangePassword(changePassword, IpAddress, Port));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : ChangePassword ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : ChangePassword ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

    }
}
