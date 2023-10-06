using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface ITrainingFeedbackFormService
    {
        Task<BaseResponse> CreateTraineeTrainingFeedbackForm(TraineeTrainingFeedbackFormCreate payload, RequesterInfo requester);
        Task<BaseResponse> CreateSupervisorTrainingFeedbackForm(SupervisorTrainingFeedbackFormCreate payload, RequesterInfo requester);
    }
}
