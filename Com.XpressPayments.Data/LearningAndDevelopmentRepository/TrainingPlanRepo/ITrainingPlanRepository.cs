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
        Task<TrainingPlanDTO> GetTrainingPlanByUserId( long UserId);
        Task<LeaveRequestDTO> GetTrainingPlanById(long TrainingPlanID);
        Task<string> ApproveTrainingPlan(long TrainingPlanID, long ApprovedByUserId);
        Task<string> DisaproveTrainingPlant(long TrainingPlanID, long DisapprovedByUserId, string DisapprovedComment);
    }
}
