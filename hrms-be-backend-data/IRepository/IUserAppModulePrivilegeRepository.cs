using hrms_be_backend_data.RepoPayload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.IRepository
{
    public interface IUserAppModulePrivilegeRepository
    {
        Task<List<GetUserAppModulePrivilegesDTO>> GetUserAppModulePrivileges();

        Task<GetUserAppModulePrivilegesDTO> GetUserAppModuleByUserandPrivilegeID(long userID, int privilegeID);

        Task<UserAppModulePrivilegesDTO> GetUserAppModulePrivilegeByID(long userAppModulePrivilegeID);

        Task<List<GetUserAppModulePrivilegesDTO>> GetUserAppModulePrivilegesByUserID(long userID);
        Task<List<AppModulePrivilegeDTO>> GetAppModulePrivilegeByAppModuleID(long ID);
        Task<List<AppModulePrivilegeDTO>> GetAppModulePrivileges();

        Task<List<GetUserAppModulePrivilegesDTO>> GetUserAppModulePrivilegesByPrivilegeID(long privilegeID);

        Task<List<GetUserAppModulePrivilegesDTO>> GetPendingUserAppModulePrivileges();
        Task<int> CreateUserAppModulePrivileges(UserAppModulePrivilegesDTO userAppModulePrivilegesDTO);

        Task<int> UpdateUserAppModulePRivileges(UserAppModulePrivilegesDTO userAppModulePrivilegesDTO);

        Task<int> ApproveUserAppModulePrivileges(UserAppModulePrivilegesDTO userAppModulePrivilegesDTO);

        Task<int> DisapproveUserAppModulePrivilege(UserAppModulePrivilegesDTO userAppModulePrivilegesDTO);
    }
}
