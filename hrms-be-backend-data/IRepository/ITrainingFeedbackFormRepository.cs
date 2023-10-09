using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface ITrainingFeedbackFormRepository
    {
        Task<string> CreateTraineeTrainingFeedbackForm(TraineeTrainingFeedbackFormCreate traineeTrainingFeedbackForm, string createdbyUserEmail);
        Task<string> CreateSupervisorTrainingFeedbackForm(SupervisorTrainingFeedbackFormCreate supervisorTrainingFeedbackForm, string createdbyUserEmail);
    }
}
