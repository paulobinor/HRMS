using hrms_be_backend_data.RepoPayload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.IRepository
{
    public interface ICompanyAppModuleRepository
    {
        Task<List<GetCompanyAppModuleByCompanyDTO>> GetCompanyAppModuleCount();
        Task<List<GetCompanyAppModuleByCompanyDTO>> GetCompanyAppModuleByCompanyID(long companyID);
        Task<int> CreateCompanyAppModule(CompanyAppModuleDTO companyAppModule);
        Task<int> UpdateCompanyAppModule(CompanyAppModuleDTO companyAppModule);
        Task<int> ApproveCompanyAppModule(CompanyAppModuleDTO companyAppModule);
        Task<AppModuleDTO> GetAppModuleByID(int appMpduleID);
        Task<List<AppModuleDTO>> GetAllAppModules();
        Task<List<GetCompanyAppModuleByCompanyDTO>> GetPendingCompanyAppModule();
        Task<int> DisapproveCompanyAppModule(CompanyAppModuleDTO companyAppModule);
        Task<GetCompanyAppModuleByCompanyDTO> GetCompanyAppModuleByCompanyandModuleID(long companyID, int moduleID);
        Task<CompanyAppModuleDTO> GetCompanyAppModuleByID(long companyAppModuleID);
    }
}
