using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface ITrainingInductionService
    {
        Task<BaseResponse> CreateTrainingInduction(TrainingInductionCreate payload, RequesterInfo requester);
        Task<BaseResponse> UpdateTrainingInduction(TrainingInductionUpdate payload, RequesterInfo requester);
        Task<BaseResponse> DeleteTrainingInduction(TrainingInductionDelete payload, RequesterInfo requester);
        Task<BaseResponse> ApproveTrainingInduction(long TrainingInductionID, RequesterInfo requester);
        Task<BaseResponse> DisaproveTrainingInduction(TrainingInductionDisapproved payload, RequesterInfo requester);
        Task<BaseResponse> GetAllTrainingInduction(RequesterInfo requester);
        Task<BaseResponse> GetAllActiveTrainingInduction(RequesterInfo requester);
        Task<BaseResponse> GetTrainingInductionById(long TrainingInductionID, RequesterInfo requester);
        Task<BaseResponse> GetTrainingInductionPendingApproval(RequesterInfo requester);
        Task<BaseResponse> GetTrainingInductionbyCompanyId(long companyId, RequesterInfo requester);
    }
}
