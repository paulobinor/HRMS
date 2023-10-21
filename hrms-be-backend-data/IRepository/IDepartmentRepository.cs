using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public interface IDepartmentRepository
    {
        Task<string> ProcessDepartment(ProcessDepartmentReq payload);
        Task<string> DeleteDepartment(DeleteDepartmentReq payload);
        Task<DepartmentWithTotalVm> GetDepartmentes(long CompanyId, int PageNumber, int RowsOfPage);
        Task<DepartmentWithTotalVm> GetDepartmentesDeleted(long CompanyId, int PageNumber, int RowsOfPage);
        Task<DepartmentVm> GetDepartmentById(long Id);
        Task<DepartmentVm> GetDepartmentByName(string DepartmentName, long CompanyId);
    }
}
