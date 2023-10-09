using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface ITrainingPlanRepository
    {
        Task<dynamic> CreateTrainingPlan(TrainingPlanCreate TrainingPlan, string createdbyUserEmail);
        Task<dynamic> UpdateTrainingPlan(TrainingPlanUpdate TrainingPlan, string updatedbyUserEmail);
        Task<dynamic> ScheduleTrainingPlan(TrainingPlanSchedule TrainingPlan, string scheduledbyUserEmail);
        Task<IEnumerable<TrainingPlanDTO>> GetAllTrainingPlan();
        Task<IEnumerable<TrainingPlanDTO>> GetAllTrainingPlanByUserId(long UserId);
        Task<IEnumerable<TrainingPlanDTO>> GetAllActiveTrainingPlan();
        Task<TrainingPlanDTO> GetTrainingPlanById(long TrainingPlanID);
        Task<string> ApproveTrainingPlan(long TrainingPlanID, long ApprovedByUserId);
        Task<string> DisaproveTrainingPlan(long TrainingPlanID, long DisapprovedByUserId, string DisapprovedComment);
        Task<dynamic> DeleteTrainingPlan(TrainingPlanDelete delete, string deletedbyUserEmail);
        Task<IEnumerable<TrainingPlanDTO>> GetTrainingPlanPendingApproval();
        Task<IEnumerable<TrainingPlanDTO>> GetTrainingPlanByCompany(long companyId);
    }
}
