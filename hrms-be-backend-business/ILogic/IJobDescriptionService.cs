using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;

namespace hrms_be_backend_business.ILogic
{
    public interface IJobDescriptionService
    {
        Task<BaseResponse> CreateJobDescription(CreateJobDescriptionDTO creatDto, RequesterInfo requester);
        Task<BaseResponse> CreateJobDescriptionBulkUpload(IFormFile payload, RequesterInfo requester);
        Task<BaseResponse> UpdateJobDescription(UpdateJobDescriptionDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteJobDescription(DeletedJobDescriptionDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveJobDescription(RequesterInfo requester);
        Task<BaseResponse> GetAllJobDescription(RequesterInfo requester);
        Task<BaseResponse> GetJobDescriptionById(long JobDescriptionID, RequesterInfo requester);
        Task<BaseResponse> GetJobDescriptionbyCompanyId(long companyId, RequesterInfo requester);
    }
}
