using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using System.Security.Claims;

namespace hrms_be_backend_business.ILogic
{
    public interface IAuthService
    {
        Task<ExecutedResult<LoginResponse>> Login(LoginModel login, string ipAddress, string port);
        Task<RefreshTokenResponse> RefreshToken(RefreshTokenModel refresh, string ipAddress, string port);
        Task<ExecutedResult<UserFullView>> CheckUserAccess(string AccessToken, string IpAddress);
        Task<BaseResponse> Logout(LogoutDto logout, string ipAddress, string port);
        Task<ExecutedResult<string>> ChangeDefaultPassword(ChangeDefaultPasswordDto payload, string ipAddress, string port);
        Task<ExecutedResult<string>> ChangePassword(ChangePasswordDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
    }
   
}
