using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface ISSOservice
    {
        //Task<BaseResponse<UserLoginDTO>> Login(UserLoginDTO user);
        Task<BaseResponse<UserLoginDTO>> AdminLogin(UserLoginDTO payload);
        Task<BaseResponse<CreateAdminUserLoginDTO>> CreateAdmin(CreateAdminUserLoginDTO payload, string Email);
    }
}