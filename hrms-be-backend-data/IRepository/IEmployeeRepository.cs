using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using System.Data;

namespace hrms_be_backend_data.IRepository
{
    public  interface IEmployeeRepository
    {
        Task<EmployeeBasisReq> ProcessEmployeeBasis(EmployeeBasisReq payload);
        Task<string> ProcessEmployeePersonalInfo(EmployeePersonalInfoReq payload);
        Task<string> ProcessEmployeeIdentification(EmployeeIdentificationReq payload);
        Task<string> ProcessEmployeeContactDetails(EmployeeContactDetailsReq payload);
        Task<string> ProcessEmployeeProfesionalBackground(EmployeeProfesionalBackgroundReq payload);
        Task<string> ProcessEmployeeReference(EmployeeReferenceReq payload);
        Task<string> ProcessEmployeeEduBackground(EmployeeEduBackgroundReq payload);
        Task<string> ProcessEmployeeBankDetails(EmployeeBankDetailsReq payload);
        Task<string> ProcessEmployeeCompensation(EmployeeCompensationReq payload);
        Task<EmployeeFullVm> GetEmployeeByEmail(string email,long companyID);

        Task<string> ApproveEmployee(long EmployeeId, string PasswordHash, long CreatedByUserId);
        Task<string> DisapproveEmployee(long Id, string Comment, long CreatedByUserId);
        Task<string> DeleteEmployee(long Id, string Comment, long CreatedByUserId);
        Task<string> CheckEmployeeStaffId(string StaffId, long CompanyId);

        Task<EmployeeWithTotalVm> GetEmployees(int PageNumber, int RowsOfPage, long AccessByUserId);
        Task<EmployeeWithTotalVm> GetEmployeesApproved(int PageNumber, int RowsOfPage, long AccessByUserId);
        Task<EmployeeWithTotalVm> GetEmployeesDisapproved(int PageNumber, int RowsOfPage, long AccessByUserId);
        Task<EmployeeWithTotalVm> GetEmployeesDeleted(int PageNumber, int RowsOfPage, long AccessByUserId);
        Task<EmployeeWithTotalVm> GetEmployeesPending(int PageNumber, int RowsOfPage, long AccessByUserId);
        Task<EmployeeSindgleVm> GetEmployeeById(long EmployeeId, long CompanyId);
        Task<EmployeeFullVm> GetEmployeeByUserId(long UserId);
        Task<List<EmployeeCertificationVm>> GetEmployeeCertification(long EmployeeId, long CompanyId);
        Task<List<EmployeeEduBackgroundVm>> GetEmployeeEduBackground(long EmployeeId, long CompanyId);
        Task<List<EmployeeIdentificationVm>> GetEmployeeIdentification(long EmployeeId, long CompanyId);
        Task<List<EmployeeProfBackgroundVm>> GetEmployeeProfBackground(long EmployeeId, long CompanyId);
        Task<List<EmployeeRefereeVm>> GetEmployeeReferee(long EmployeeId, long CompanyId);
        Task<int> AddEmployeeBulk(DataTable dataTable, RequesterInfo requester, long currentStaffCount, int listCount, long companyID);
    }
}
