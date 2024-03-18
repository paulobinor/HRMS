using hrms_be_backend_common.Communication;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface ITrainingPlanService
    {
        Task<ExecutedResult<string>> CreateTrainingPlan(TrainingPlanCreate payload, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<string>> UpdateTrainingPlan(TrainingPlanUpdate payload, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<string>> ScheduleTrainingPlan(TrainingPlanSchedule payload, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<string>> DeleteTrainingPlan(TrainingPlanDelete payload, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<string>> ApproveTrainingPlan(TrainingPlanApproved payload, string AccessKey, string RemoteIpAddressy);
        Task<ExecutedResult<string>> DisapproveTrainingPlan(TrainingPlanDisapproved payload, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<TrainingPlanDTO>>> GetAllTrainingPlan(string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<TrainingPlanDTO>>> GetAllTrainingPlanByUserId(long UserID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<TrainingPlanDTO>>> GetAllActiveTrainingPlan(string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<TrainingPlanDTO>> GetTrainingPlanById(long TrainingPlanID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<TrainingPlanDTO>>> GetTrainingPlanPendingApproval(string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<TrainingPlanDTO>>> GetTrainingPlanbyCompanyId(long companyId, string AccessKey, string RemoteIpAddress);
    }
}
