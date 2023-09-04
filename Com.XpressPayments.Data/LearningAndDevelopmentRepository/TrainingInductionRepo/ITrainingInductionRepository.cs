using Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.LearningAndDevelopmentRepository.TrainingInductionRepo
{
    public interface ITrainingInductionRepository
    {
        Task<string> CreateTrainingInduction(TrainingInductionCreate TrainingInduction, string createdbyUserEmail);
        Task<dynamic> UpdateTrainingInduction(TrainingInductionUpdate TrainingInduction, string updatedbyUserEmail);
        Task<IEnumerable<TrainingInductionDTO>> GetAllTrainingInduction();
        Task<IEnumerable<TrainingInductionDTO>> GetAllActiveTrainingInduction();
        Task<TrainingInductionDTO> GetTrainingInductionById(long TrainingInductionID);
        Task<string> ApproveTrainingInduction(long TrainingInductionID, long ApprovedByUserId);
        Task<string> DisaproveTrainingInduction(long TrainingInductionID, long DisapprovedByUserId, string DisapprovedComment);
        Task<dynamic> DeleteTrainingInduction(TrainingInductionDelete delete, string deletedbyUserEmail);
        Task<IEnumerable<TrainingInductionDTO>> GetTrainingInductionPendingApproval();
        Task<IEnumerable<TrainingInductionDTO>> GetTrainingInductionByCompany(long companyId);
    }
}
