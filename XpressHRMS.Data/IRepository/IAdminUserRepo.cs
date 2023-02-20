using System.Collections.Generic;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Data.IRepository
{
    public interface IAdminUserRepo
    {
        Task<dynamic> CreateAdminUser(CreateAdminUserLoginDTO payload);
        Task<int> UpdateAdminUser(UpdateAdminUserLoginDTO payload);
        Task<int> DeleteAdminUser(int CompanyID, int AdminUserID);
        Task<int> ActivateAdminUser(int CompanyID, int AdminUserID);
        Task<int> DisableAdminUser(int CompanyID, int AdminUserID);
        Task<AdminDTO> LoginAdmin(UserLoginDTO payload);
        Task<IEnumerable<AdminDTO>> GetUser(UserLoginDTO payload);
        Task<List<GetAllAdminUserLoginDTO>> GetAllAdminUser();
        //Task<IEnumerable<AdminDTO>> GetAdminUser(UserLoginDTO payload);
        //Task<IEnumerable<AdminDTO>> GetUser(UserLoginDTO payload);
    }
}