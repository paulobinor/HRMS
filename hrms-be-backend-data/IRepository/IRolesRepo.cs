using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface IRolesRepo
    {
        Task<IEnumerable<RolesDTO>> GetAllRoles();
        Task<RolesDTO> GetRolesByName(string RoleName);
    }
}
