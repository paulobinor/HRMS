using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_business.ILogic
{
    public interface IUserAppModulePrivilegeService
    {

        Task<BaseResponse> GetAppModulePrivileges(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> GetAppModulePrivilegesByAppModuleID(long appModulePrivilegeID, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> GetUserAppModulePrivileges(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> GetPendingUserAppModulePRivilege(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> GetUserAppModulePrivilegesByUserID(long userID, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> CreateUserAppModulePrivileges(CreateUserAppModulePrivilegesDTO createUserAppModulePrivileges, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> ApproveUserAppModulePrivilege(long userAppModulePrivilegeID, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> DisapproveUserAppModulePrivilage(long userAppModulePrivilegeID, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> UserAppModulePrivilegeActivationSwitch(long userAppModulePrivilegeID, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> DeleteUserAppModulePrivilege(long userAppModulePrivilegeID, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
    }
}
