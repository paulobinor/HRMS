using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.LearningAndDevelopmentModuleService.Service.ILogic
{
    public interface ITrainingPlanService
    {
        Task<BaseResponse> CreateTrainingPlan(TrainingPlanCreate payload, RequesterInfo requester);
        Task<BaseResponse> UpdateTrainingPlan(TrainingPlanUpdate payload, RequesterInfo requester);
        Task<BaseResponse> ScheduleTrainingPlan(TrainingPlanSchedule payload, RequesterInfo requester);
        Task<BaseResponse> DeleteTrainingPlan(TrainingPlanDelete payload, RequesterInfo requester);
        Task<BaseResponse> ApproveTrainingPlan(TrainingPlanApproved payload, RequesterInfo requester);
        Task<BaseResponse> DisaproveTrainingPlan(TrainingPlanDisapproved payload, RequesterInfo requester);
        Task<BaseResponse> GetAllTrainingPlan(RequesterInfo requester);
        Task<BaseResponse> GetAllTrainingPlanByUserId(long UserID,RequesterInfo requester);
        Task<BaseResponse> GetAllActiveTrainingPlan(RequesterInfo requester);
        Task<BaseResponse> GetTrainingPlanById(long TrainingPlanID, RequesterInfo requester);
        Task<BaseResponse> GetTrainingPlanPendingApproval(RequesterInfo requester);
        Task<BaseResponse> GetTrainingPlanbyCompanyId(long companyId, RequesterInfo requester);
    }
}
