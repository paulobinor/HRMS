using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs;
using Com.XpressPayments.Data.LeaveModuleDTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Com.XpressPayments.Data.LearningAndDevelopmentRepository.TrainingPlanRepo
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
