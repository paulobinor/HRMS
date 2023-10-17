using hrms_be_backend_common.Communication;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginModel login, string ipAddress, string port);
        Task<RefreshTokenResponse> RefreshToken(RefreshTokenModel refresh, string ipAddress, string port);
        Task<ExecutedResult<UserFullView>> CheckUserAccess(string AccessToken, string IpAddress);
        Task<BaseResponse> Logout(LogoutDto logout, string ipAddress, string port);
        Task<BaseResponse> ChangePassword(ChangePasswordViewModel changePassword, string ipAddress, string port);
        Task<BaseResponse> SendEmailForPasswordChange(RequestPasswordChange request, string ipAddress, string port);


    }

   
}
