using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_business.ILogic
{
    public interface IUserAppModulePrivilegeService
    {

        Task<BaseResponse> GetAppModulePrivileges(RequesterInfo requester);
        Task<BaseResponse> GetAppModulePrivilegesByAppModuleID(long appModulePrivilegeID, RequesterInfo requester);
        Task<BaseResponse> GetUserAppModulePrivileges(RequesterInfo requester);
        Task<BaseResponse> GetPendingUserAppModulePRivilege(RequesterInfo requester);
        Task<BaseResponse> GetUserAppModulePrivilegesByUserID(long userID, RequesterInfo requester);
        Task<BaseResponse> CreateUserAppModulePrivileges(CreateUserAppModulePrivilegesDTO createUserAppModulePrivileges, RequesterInfo requester);
        Task<BaseResponse> ApproveUserAppModulePrivilege(long userAppModulePrivilegeID, RequesterInfo requester);
        Task<BaseResponse> DisapproveUserAppModulePrivilage(long userAppModulePrivilegeID, RequesterInfo requester);
        Task<BaseResponse> UserAppModulePrivilegeActivationSwitch(long userAppModulePrivilegeID, RequesterInfo requester);
        Task<BaseResponse> DeleteUserAppModulePrivilege(long userAppModulePrivilegeID, RequesterInfo requester);
    }
}
