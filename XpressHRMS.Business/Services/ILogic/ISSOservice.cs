using System.Collections.Generic;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface ISSOservice
    {
        //Task<BaseResponse<UserLoginDTO>> Login(UserLoginDTO user);
        Task<BaseResponse<UserLoginDTO>> AdminLogin(UserLoginDTO payload);
        Task<BaseResponse> CreateAdmin(CreateAdminUserLoginDTO payload, string Email);
        Task<BaseResponse<UpdateAdminUserLoginDTO>> UpdateAdmin(UpdateAdminUserLoginDTO UpdateAdmin, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse<int>> DeleteAdmin(int CompanyID, int AdminUserID, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse<int>> DisableAdmin(int CompanyID, int AdminUserID, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse<int>> ActivateAdminUser(int CompanyID, int AdminUserID, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse<List<GetAllAdminUserLoginDTO>>> GetAllAdminUser();
    }
}