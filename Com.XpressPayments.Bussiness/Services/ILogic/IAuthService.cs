using Com.XpressPayments.Bussiness.ViewModels;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.DTOs.Account;
using Com.XpressPayments.Data.GenericResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.ILogic
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginModel login, string ipAddress, string port);
        Task<RefreshTokenResponse> RefreshToken(RefreshTokenModel refresh, string ipAddress, string port);
        Task<BaseResponse> Logout(LogoutDto logout, string ipAddress, string port);
        Task<BaseResponse> CreateUser(CreateUserDto userDto, RequesterInfo requester);
        Task<BaseResponse> UpdateUser(UpdateUserDto updateDto, RequesterInfo requester);
        Task<BaseResponse> SendEmailForPasswordChange(RequestPasswordChange request, string ipAddress, string port);
        Task<BaseResponse> ChangePassword(ChangePasswordViewModel changePassword, string ipAddress, string port);
        Task<BaseResponse> GetAllUsers(RequesterInfo requester);
        Task<BaseResponse> GetAllUsersPendingApproval(RequesterInfo requester);
        Task<BaseResponse> ApproveUser(ApproveUserDto approveUser, RequesterInfo requester);
        Task<BaseResponse> DisapproveUser(DisapproveUserDto disapproveUser, RequesterInfo requester);
        Task<BaseResponse> DeactivateUser(DeactivateUserDto deactivateUser, RequesterInfo requester);
        Task<BaseResponse> ReactivateUser(ReactivateUserDto reactivateUser, RequesterInfo requester);
        Task<BaseResponse> UnblockAccount(UnblockAccountDto unblockUser, RequesterInfo requester);
        Task<BaseResponse> GetAllUsersbyDeptId(long DepartmentId, RequesterInfo requester);
        
    }

   
}
