using System.Collections.Generic;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Data.IRepository
{
    public interface IRoleRepository
    {
        Task<int> CreateRole(CreateRoleDTO payload);
        Task<int> DeleteRole(DeleteRoleDTO payload);
        Task<List<RoleDTO>> GetAllRoles(int CompanyID);
        Task<RoleDTO> GetRolesByID(DeleteRoleDTO payload);
        Task<int> UpdateRole(UpdateRoleDTO payload);
    }
}