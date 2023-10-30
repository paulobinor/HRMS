using hrms_be_backend_data.RepoPayload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.IRepository
{
    public interface IDepartmentalModulesRepository
    {
        Task<List<GetDepartmentalModuleCount>> GetDepartmentalAppModuleCount(long compnayID);
        Task<List<GetDepartmentalModuleCount>> GetAllDepartmentalAppModuleCount(long compnayID);
        Task<List<GetDepartmentalModuleCount>> GetDisapprovedDepartmentalAppModuleCount(long compnayID);
        Task<GetDepartmentModuleByDepartmentDTO> GetDepartmentalAppModuleByDepartmentandModuleID(long departmentID, int moduleID);
        Task<DepartmentalModulesDTO> GetDepartmentalAppModuleByID(long departmentalAppModuleID);
        Task<List<GetDepartmentModuleByDepartmentDTO>> GetDepartmentalAppModuleByDepartmentID(long departmentID);
        Task<List<GetDepartmentModuleByDepartmentDTO>> GetPendingDepartmentalAppModule(long compnayID);
        Task<string> CreateDepartmentalAppModule(DepartmentalModulesReq departmentalAppModule);
        Task<long> CreateDepartmentalAppModule(DepartmentalModulesDTO departmentalAppModule);
        Task<long> UpdateDepartmentAppModule(DepartmentalModulesDTO departmentalAppModule);
        Task<long> ApproveDepartmentalAppModule(DepartmentalModulesDTO departmentalAppModule);
        Task<long> DisapproveDepartmentalAppModule(DepartmentalModulesDTO departmentalAppModule);
    }
}
