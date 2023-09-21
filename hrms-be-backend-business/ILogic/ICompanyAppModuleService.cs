using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_business.ILogic
{
    public interface ICompanyAppModuleService
    {
        Task<BaseResponse> GetAllAppModules(RequesterInfo requester);
        Task<BaseResponse> GetCompanyAppModuleCount(RequesterInfo requester);
        Task<BaseResponse> GetPendingCompanyAppModule(RequesterInfo requester);
        Task<BaseResponse> GetCompanyAppModuleByCompanyID(long companyID, RequesterInfo requester);
        Task<BaseResponse> CreateCompanyAppModule(CreateCompanyAppModuleDTO createAppModule, RequesterInfo requester);
        Task<BaseResponse> ApproveCompanyAppModule(long companyAppModuleID, RequesterInfo requester);
        Task<BaseResponse> DisapproveCompanyAppModule(long companyAppModuleID, RequesterInfo requester);
        Task<BaseResponse> CompanyAppModuleActivationSwitch(long companyAppModuleID, RequesterInfo requester);
        Task<BaseResponse> DeleteCompanyAppModule(long companyAppModuleID, RequesterInfo requester);
    }
}
