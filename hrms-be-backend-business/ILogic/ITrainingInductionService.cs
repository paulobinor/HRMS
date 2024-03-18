using hrms_be_backend_common.Communication;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface ITrainingInductionService
    {
        Task<ExecutedResult<string>> CreateTrainingInduction(TrainingInductionCreate payload, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<string>> UpdateTrainingInduction(TrainingInductionUpdate payload, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<string>> DeleteTrainingInduction(TrainingInductionDelete payload, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<string>> ApproveTrainingInduction(long TrainingInductionID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<string>> DisaproveTrainingInduction(TrainingInductionDisapproved payload, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<TrainingInductionDTO>>> GetAllTrainingInduction(string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<TrainingInductionDTO>>> GetAllActiveTrainingInduction(string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<TrainingInductionDTO>> GetTrainingInductionById(long TrainingInductionID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<TrainingInductionDTO>>> GetTrainingInductionPendingApproval(string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<TrainingInductionDTO>>> GetTrainingInductionbyCompanyId(long companyId, string AccessKey, string RemoteIpAddress);
    }
}
