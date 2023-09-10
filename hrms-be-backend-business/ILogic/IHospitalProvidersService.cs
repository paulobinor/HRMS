using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;

namespace hrms_be_backend_business.ILogic
{
    public interface IHospitalProvidersService
    {
        Task<BaseResponse> CreateHospitalProviders(CreateHospitalProvidersDTO create, RequesterInfo requester);
        Task<BaseResponse> CreateHospitalProvidersBulkUpload(IFormFile payload, RequesterInfo requester);
        Task<BaseResponse> UpdateHospitalProviders(UpdateHospitalProvidersDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteHospitalProviders(DeleteHospitalProvidersDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveHospitalProviders(RequesterInfo requester);
        Task<BaseResponse> GetAllHospitalProviders(RequesterInfo requester);
        Task<BaseResponse> GetHospitalProvidersbyId(long ID, RequesterInfo requester);
        Task<BaseResponse> GetHospitalProvidersbyCompanyId(long companyId, RequesterInfo requester);

    }
}
