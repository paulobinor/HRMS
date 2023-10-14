using hrms_be_backend_common.Communication;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace hrms_be_backend_business.ILogic
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginModel login, string ipAddress, string port);
        Task<RefreshTokenResponse> RefreshToken(RefreshTokenModel refresh, string ipAddress, string port);
        Task<ExecutedResult<User>> CheckUserAccess(string AccessToken, string IpAddress);
        Task<BaseResponse> Logout(LogoutDto logout, string ipAddress, string port);
        Task<BaseResponse> CreateUser(CreateUserDto userDto, RequesterInfo requester);
        Task<BaseResponse> CreateUserBulkUpload(IFormFile payload, RequesterInfo requester);
        Task<BaseResponse> CreateUserBulkUploadTwo(IFormFile payload, long companyID, RequesterInfo requester);
        Task<BaseResponse> UpdateUser(UpdateUserDto updateDto, RequesterInfo requester);
        Task<BaseResponse> SendEmailForPasswordChange(RequestPasswordChange request, string ipAddress, string port);
        Task<BaseResponse> ChangePassword(ChangePasswordViewModel changePassword, string ipAddress, string port);
        Task<BaseResponse> GetAllUsers(RequesterInfo requester);
        Task<BaseResponse> GetAllUsersPendingApproval(long CompanyId, RequesterInfo requester);
        Task<BaseResponse> ApproveUser(ApproveUserDto approveUser, RequesterInfo requester);
        Task<BaseResponse> DisapproveUser(DisapproveUserDto disapproveUser, RequesterInfo requester);
        Task<BaseResponse> DeactivateUser(DeactivateUserDto deactivateUser, RequesterInfo requester);
        Task<BaseResponse> ReactivateUser(ReactivateUserDto reactivateUser, RequesterInfo requester);
        Task<BaseResponse> UnblockAccount(UnblockAccountDto unblockUser, RequesterInfo requester);
        Task<BaseResponse> GetAllUsersbyDeptId(long DepartmentId, RequesterInfo requester);
        
    }

   
}
