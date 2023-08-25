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
        Task<string> CreateTrainingPlan(TrainingPlanCreate TrainingPlan);
        Task<IEnumerable<TrainingPlanDTO>> GetAllTrainingPlan();
        Task<TrainingPlanDTO> GetTrainingPlanById(long TrainingPlanID);
        Task<string> ApproveTrainingPlan(long TrainingPlanID, long ApprovedByUserId);
        Task<string> DisaproveTrainingPlan(long TrainingPlanID, long DisapprovedByUserId, string DisapprovedComment);
        Task<dynamic> DeleteTrainingPlan(TrainingPlanDelete delete, string deletedbyUserEmail);
        Task<IEnumerable<TrainingPlanDTO>> GetTrainingPlanPendingApproval(long UserIdGet);
        Task<IEnumerable<TrainingPlanDTO>> GetTrainingPlanByCompany(long companyId);
    }
}
