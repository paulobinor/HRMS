using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public interface IEmployeeTypeRepository
    {
        Task<string> ProcessEmployeeType(ProcessEmployeeTypeReq payload);
        Task<string> DeleteEmployeeType(DeleteEmployeeTypeReq payload);
        Task<EmployeeTypeWithTotalVm> GetEmployeeTypes(long CompanyId, int PageNumber, int RowsOfPage);
        Task<EmployeeTypeWithTotalVm> GetEmployeeTypesDeleted(long CompanyId, int PageNumber, int RowsOfPage);
        Task<EmployeeTypeVm> GetEmployeeTypeById(long Id);
        Task<EmployeeTypeVm> GetEmployeeTypeByName(string EmployeeTypeName, long CompanyId);
    }
}
