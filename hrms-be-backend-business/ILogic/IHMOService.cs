using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface IHMOService
    {
        Task<BaseResponse> CreateHMO(CreateHMODTO HomDto, RequesterInfo requester);
        Task<BaseResponse> UpdateHMO(UpdateHMODTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteHMO(DeleteHMODTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveHMO(RequesterInfo requester);
        Task<BaseResponse> GetAllHMO(RequesterInfo requester);
        Task<BaseResponse> GetHMObyId(long ID, RequesterInfo requester);
        Task<BaseResponse> GetHMObyCompanyId(long companyId, RequesterInfo requester);
    }
}
