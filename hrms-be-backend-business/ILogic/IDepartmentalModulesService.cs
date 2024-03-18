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
    public interface IDepartmentalModulesService
    {
        Task<BaseResponse> GetDepartmentalAppModuleCount(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> GetPendingDepartmentalAppModule(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> GetDepartmentalAppModuleByDepartmentID(long departmentIDstring ,string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> CreateDepartmentalAppModule(CreateDepartmentalModuleDTO createDepartmentalAppModule, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> ApproveDepartmentalAppModule(ApproveDepartmentalModules request, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> DisapproveDepartmentalAppModule(ApproveDepartmentalModules request, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> DeleteDepartmentAppModule(long departmentAppModuleID, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> GetDepartmentAppModuleStatus(GetByStatus status, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> DepartmentAppModuleActivationSwitch(long departmentAppModuleID, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
    }
}
