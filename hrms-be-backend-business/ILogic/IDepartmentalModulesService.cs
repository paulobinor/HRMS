using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static hrms_be_backend_business.Logic.CompanyAppModuleService;

namespace hrms_be_backend_business.ILogic
{
    public interface IDepartmentalModulesService
    {
        Task<BaseResponse> GetDepartmentalAppModuleCount(RequesterInfo requester);
        Task<BaseResponse> GetPendingDepartmentalAppModule(RequesterInfo requester);
        Task<BaseResponse> GetDepartmentalAppModuleByDepartmentID(long departmentID, RequesterInfo requester);
        Task<BaseResponse> CreateDepartmentalAppModule(CreateDepartmentalModuleDTO createDepartmentalAppModule, RequesterInfo requester);
        Task<BaseResponse> ApproveDepartmentalAppModule(long departmentAppModuleID, RequesterInfo requester);
        Task<BaseResponse> DisapproveDepartmentalAppModule(long departmentAppModuleID, RequesterInfo requester);
        Task<BaseResponse> DeleteDepartmentAppModule(long departmentAppModuleID, RequesterInfo requester);
        Task<BaseResponse> GetDepartmentAppModuleStatus(GetByStatus status, RequesterInfo requester);
        Task<BaseResponse> DepartmentAppModuleActivationSwitch(long departmentAppModuleID, RequesterInfo requester);
    }
}
