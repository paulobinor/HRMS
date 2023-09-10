using hrms_be_backend_data.RepoPayload.OnBoardingDTO.DTOs;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface IReviwerService
    {
        Task<BaseResponse> CreateReviwer(CreateReviwerDTO create, RequesterInfo requester);
        Task<BaseResponse> DeleteReviwer(DeleteReviwerDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetReviwerbyId(long UserId, RequesterInfo requester);
        Task<BaseResponse> GetReviwerbyCompanyId(long companyId, RequesterInfo requester);
    }
}
