using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface ITrainingInductionRepository
    {
        Task<dynamic> CreateTrainingInduction(TrainingInductionCreate TrainingInduction, string createdbyUserEmail);
        Task<dynamic> UpdateTrainingInduction(TrainingInductionUpdate TrainingInduction, string updatedbyUserEmail);
        Task<IEnumerable<TrainingInductionDTO>> GetAllTrainingInduction();
        Task<IEnumerable<TrainingInductionDTO>> GetAllActiveTrainingInduction();
        Task<TrainingInductionDTO> GetTrainingInductionById(long TrainingInductionID);
        Task<string> ApproveTrainingInduction(long TrainingInductionID, long ApprovedByUserId);
        Task<string> DisapproveTrainingInduction(long TrainingInductionID, long DisapprovedByUserId, string DisapprovedComment);
        Task<dynamic> DeleteTrainingInduction(TrainingInductionDelete delete, string deletedbyUserEmail);
        Task<IEnumerable<TrainingInductionDTO>> GetTrainingInductionPendingApproval();
        Task<IEnumerable<TrainingInductionDTO>> GetTrainingInductionByCompany(long companyId);
    }
}
