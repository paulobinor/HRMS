using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;

namespace hrms_be_backend_business.ILogic
{
    public  interface IUnitHeadService
    {
        Task<BaseResponse> CreateUnitHead(CreateUnitHeadDTO creatDto, RequesterInfo requester);
        Task<BaseResponse> CreateUnitHeadBulkUpload(IFormFile payload, RequesterInfo requester);
        Task<BaseResponse> UpdateUnitHead(UpdateUnitHeadDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteUnitHead(DeleteUnitHeadDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveUnitHead(RequesterInfo requester);
        Task<BaseResponse> GetAllUnitHead(RequesterInfo requester);
        Task<BaseResponse> GetUnitHeadById(long UnitHeadID, RequesterInfo requester);
        Task<BaseResponse> GetUnitHeadbyCompanyId(long companyId, RequesterInfo requester);
    }
}
