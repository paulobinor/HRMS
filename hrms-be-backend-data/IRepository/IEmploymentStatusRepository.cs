using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public  interface IEmploymentStatusRepository
    {
        Task<string> ProcessEmploymentStatus(ProcessEmploymentStatusReq payload);
        Task<string> DeleteEmploymentStatus(DeleteEmploymentStatusReq payload);
        Task<EmploymentStatusWithTotalVm> GetEmploymentStatus(long CompanyId, int PageNumber, int RowsOfPage);
        Task<EmploymentStatusWithTotalVm> GetEmploymentStatusDeleted(long CompanyId, int PageNumber, int RowsOfPage);
        Task<EmploymentStatusVm> GetEmploymentStatusById(long Id);
        Task<EmploymentStatusVm> GetEmploymentStatusByName(string EmploymentStatusName, long CompanyId);
    }
}
