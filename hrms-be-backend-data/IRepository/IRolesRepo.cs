using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface IRolesRepo
    {
        Task<RolesDTO> GetRolesByName(string RoleName);
    }
}
