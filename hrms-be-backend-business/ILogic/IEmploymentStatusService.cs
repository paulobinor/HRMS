using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;

namespace hrms_be_backend_business.ILogic
{
    public  interface IEmploymentStatusService
    {
        Task<BaseResponse> CreateEmploymentStatus(CreateEmploymentStatusDTO creatDto, RequesterInfo requester);
        Task<BaseResponse> CreateEmploymentStatusBulkUpload(IFormFile payload, RequesterInfo requester);
        Task<BaseResponse> UpdateEmploymentStatus(UpdateEmploymentStatusDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteEmploymentStatus(DeleteEmploymentStatusDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveEmploymentStatus(RequesterInfo requester);
        Task<BaseResponse> GetAllEmploymentStatus(RequesterInfo requester);
        Task<BaseResponse> GetEmploymentStatusbyId(long EmploymentStatusID, RequesterInfo requester);
        Task<BaseResponse> GetEmpLoymentStatusbyCompanyId(long CompanyID, RequesterInfo requester);
    }
}
