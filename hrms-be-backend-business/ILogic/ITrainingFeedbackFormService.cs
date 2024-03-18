using hrms_be_backend_common.Communication;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface ITrainingFeedbackFormService
    {
        Task<ExecutedResult<string>> CreateTraineeTrainingFeedbackForm(TraineeTrainingFeedbackFormCreate payload, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<string>> CreateSupervisorTrainingFeedbackForm(SupervisorTrainingFeedbackFormCreate payload, string AccessKey, string RemoteIpAddress);
    }
}
