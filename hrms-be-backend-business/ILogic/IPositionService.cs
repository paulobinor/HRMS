using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;

namespace hrms_be_backend_business.ILogic
{
    public interface IPositionService
    {
        Task<BaseResponse> CreatePosition(CreatePositionDTO creatDto, RequesterInfo requester);
        Task<BaseResponse> CreatePositionBulkUpload(IFormFile payload, RequesterInfo requester);
        Task<BaseResponse> UpdatePosition(UpadtePositionDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeletePosition(DeletePositionDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActivePosition(RequesterInfo requester);
        Task<BaseResponse> GetAllPosition(RequesterInfo requester);
        Task<BaseResponse> GetPositionById(long PositionID, RequesterInfo requester);
        Task<BaseResponse> GetPositionbyCompanyId(long companyId, RequesterInfo requester);
    }
}
