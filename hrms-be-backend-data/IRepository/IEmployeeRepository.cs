using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public  interface IEmployeeRepository
    {
        Task<string> ProcessEmployeeBasis(ProcessEmployeeBasisReq payload);
        Task<string> ApproveEmployee(long Id, long CreatedByUserId);
        Task<string> DisapproveEmployee(long Id, string Comment, long CreatedByUserId);
        Task<string> DeleteEmployee(long Id, string Comment, long CreatedByUserId);
        Task<EmployeeWithTotalVm> GetEmployees(int PageNumber, int RowsOfPage, long AccessByUserId);
        Task<EmployeeWithTotalVm> GetEmployeesApproved(int PageNumber, int RowsOfPage, long AccessByUserId);
        Task<EmployeeWithTotalVm> GetEmployeesDisapproved(int PageNumber, int RowsOfPage, long AccessByUserId);
        Task<EmployeeWithTotalVm> GetEmployeesDeleted(int PageNumber, int RowsOfPage, long AccessByUserId);
        Task<EmployeeFullVm> GetEmployeeById(long Id);
        Task<EmployeeFullVm> GetEmployeeByUserId(long UserId);
    }
}
