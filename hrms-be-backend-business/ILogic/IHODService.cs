using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;

namespace hrms_be_backend_business.ILogic
{
    public  interface IHODService
    {
        Task<BaseResponse> CreateHOD(CreateHodDTO HodDto, RequesterInfo requester);
        Task<BaseResponse> CreateHODBulkUpload(IFormFile payload, RequesterInfo requester);
        Task<BaseResponse> UpdateHOD(UpdateHodDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteHOD(DeleteHodDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveHOD(RequesterInfo requester);
        Task<BaseResponse> GetAllHOD(RequesterInfo requester);
        Task<BaseResponse> GetHODbyId(long HodID, RequesterInfo requester);
        Task<BaseResponse> GetHODbyCompanyId(long companyId, RequesterInfo requester);

    }
}
