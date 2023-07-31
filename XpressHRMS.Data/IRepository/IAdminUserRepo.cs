using System.Collections.Generic;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Data.IRepository
{
    public interface IAdminUserRepo
    {
        Task<dynamic> CreateAdminUser(CreateAdminUserLoginDTO payload);
        Task<AdminDTO> LoginAdmin(UserLoginDTO payload
           );
        Task<IEnumerable<AdminDTO>> GetAdminUser(UserLoginDTO payload);
    }
}