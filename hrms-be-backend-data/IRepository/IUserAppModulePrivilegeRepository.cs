using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface IUserAppModulePrivilegeRepository
    {
        Task<string> CheckUserAppPrivilege(string PrivilegeCode, long CreatedByUserId);
        Task<List<GetUserAppModulePrivilegesDTO>> GetUserAppModulePrivileges(long companyID);

        Task<GetUserAppModulePrivilegesDTO> GetUserAppModuleByUserandPrivilegeID(long userID, int privilegeID);

        Task<UserAppModulePrivilegesDTO> GetUserAppModulePrivilegeByID(long userAppModulePrivilegeID);

        Task<List<GetUserAppModulePrivilegesDTO>> GetUserAppModulePrivilegesByUserID(long userID);
        Task<List<AppModulePrivilegeDTO>> GetAppModulePrivilegeByAppModuleID(long ID);
        Task<List<AppModulePrivilegeDTO>> GetAppModulePrivileges();

        Task<List<GetUserAppModulePrivilegesDTO>> GetUserAppModulePrivilegesByPrivilegeID(long privilegeID);

        Task<List<GetUserAppModulePrivilegesDTO>> GetPendingUserAppModulePrivileges(long companyID);
        Task<int> CreateUserAppModulePrivileges(UserAppModulePrivilegesDTO userAppModulePrivilegesDTO);

        Task<int> UpdateUserAppModulePRivileges(UserAppModulePrivilegesDTO userAppModulePrivilegesDTO);

        Task<int> ApproveUserAppModulePrivileges(UserAppModulePrivilegesDTO userAppModulePrivilegesDTO);

        Task<int> DisapproveUserAppModulePrivilege(UserAppModulePrivilegesDTO userAppModulePrivilegesDTO);
    }
}
