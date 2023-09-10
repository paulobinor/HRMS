using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;

namespace hrms_be_backend_business.ILogic
{
    public  interface IGradeService
    {
        Task<BaseResponse> CreateGrade(CreateGradeDTO creatDto, RequesterInfo requester);
        Task<BaseResponse> CreateGradeBulkUpload(IFormFile payload, RequesterInfo requester);
        Task<BaseResponse> UpdateGrade(UpdateGradeDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteGrade(DeleteGradeDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveGrade(RequesterInfo requester);
        Task<BaseResponse> GetAllGrade(RequesterInfo requester);
        Task<BaseResponse> GetGradeById(long GradeID, RequesterInfo requester);
        Task<BaseResponse> GetGradebyCompanyId(long companyId, RequesterInfo requester);
    }
}
