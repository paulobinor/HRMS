using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;

namespace hrms_be_backend_business.ILogic
{
    public  interface IUnitService
    {
        Task<BaseResponse> CreateUnit(CreateUnitDTO unitDto, RequesterInfo requester);
        Task<BaseResponse> CreateUnitBulkUpload(IFormFile payload, RequesterInfo requester);
        Task<BaseResponse> UpdateUnit(UpdateUnitDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteUnit(DeleteUnitDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveUnit(RequesterInfo requester);
        Task<BaseResponse> GetAllUnit(RequesterInfo requester);
        Task<BaseResponse> GetUnitById(long UnitID, RequesterInfo requester);
        Task<BaseResponse> GetUnitbyCompanyId(long companyId, RequesterInfo requester);
    }
}
