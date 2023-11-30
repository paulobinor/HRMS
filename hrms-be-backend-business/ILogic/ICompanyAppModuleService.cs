using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static hrms_be_backend_business.Logic.CompanyAppModuleService;

namespace hrms_be_backend_business.ILogic
{
    public interface ICompanyAppModuleService
    {
        Task<BaseResponse> GetAllAppModules(RequesterInfo requester);
        Task<BaseResponse> GetCompanyAppModuleStatus(GetByStatus status, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> GetCompanyAppModuleCount(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> GetPendingCompanyAppModule(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> GetCompanyAppModuleByCompanyID(long companyID, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> CreateCompanyAppModule(CreateCompanyAppModuleDTO createAppModule, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> ApproveCompanyAppModule(ApproveCompanyAppModulesRequest request, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> DisapproveCompanyAppModule(ApproveCompanyAppModulesRequest request, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> CompanyAppModuleActivationSwitch(long companyAppModuleID, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> DeleteCompanyAppModule(long companyAppModuleID, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
    }
}
