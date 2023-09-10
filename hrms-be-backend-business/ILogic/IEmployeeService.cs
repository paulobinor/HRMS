using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface IEmployeeService
    {
        Task<BaseResponse> UpdateEmployee(UpdateEmployeeDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveEmployee(RequesterInfo requester);
        Task<BaseResponse> GetAllEmployee(RequesterInfo requester);
        Task<BaseResponse> GetEmployeeById(long EmpID, RequesterInfo requester);
        Task<BaseResponse> GetEmployeebyCompanyId(long companyId, RequesterInfo requester);
        Task<BaseResponse> GetEmpPendingApproval(long CompanyID, RequesterInfo requester);
        Task<BaseResponse> ApproveEmp(ApproveEmp approveEmp, RequesterInfo requester);
        Task<BaseResponse> DisapproveEmp(DisapproveEmpDto disapproveEmp, RequesterInfo requester);
    }
}
