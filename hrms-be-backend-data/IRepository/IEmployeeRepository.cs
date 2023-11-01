using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using System.Data;

namespace hrms_be_backend_data.IRepository
{
    public  interface IEmployeeRepository
    {
        Task<string> ProcessEmployeeBasis(ProcessEmployeeBasisReq payload);
        Task<string> ProcessEmployeePersonalInfo(ProcessEmployeePersonalInfoReq payload);
        Task<string> ProcessEmployeeIdentification(ProcessEmployeeIdentificationReq payload);
        Task<string> ProcessEmployeeContactDetails(ProcessEmployeeContactDetailsReq payload);

        Task<string> ApproveEmployee(long Id, long CreatedByUserId);
        Task<string> DisapproveEmployee(long Id, string Comment, long CreatedByUserId);
        Task<string> DeleteEmployee(long Id, string Comment, long CreatedByUserId);
        Task<EmployeeWithTotalVm> GetEmployees(int PageNumber, int RowsOfPage, long AccessByUserId);
        Task<EmployeeWithTotalVm> GetEmployeesApproved(int PageNumber, int RowsOfPage, long AccessByUserId);
        Task<EmployeeWithTotalVm> GetEmployeesDisapproved(int PageNumber, int RowsOfPage, long AccessByUserId);
        Task<EmployeeWithTotalVm> GetEmployeesDeleted(int PageNumber, int RowsOfPage, long AccessByUserId);
        Task<EmployeeFullVm> GetEmployeeById(long Id);
        Task<EmployeeFullVm> GetEmployeeByUserId(long UserId);
        Task<int> AddEmployeeBulk(DataTable dataTable, RequesterInfo requester, long currentStaffCount, int listCount, long companyID);
        Task<EmployeeFullVm> GetEmployeeByEmail(string email);
    }
}
