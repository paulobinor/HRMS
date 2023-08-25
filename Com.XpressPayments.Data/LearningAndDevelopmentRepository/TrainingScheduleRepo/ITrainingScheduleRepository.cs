using Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.LearningAndDevelopmentRepository.TrainingScheduleRepo
{
    public interface ITrainingScheduleRepository
    {
        Task<string> CreateTrainingSchedule(TrainingScheduleCreate TrainingSchedule);
        Task<IEnumerable<TrainingScheduleDTO>> GetAllTrainingSchedule();
        Task<TrainingScheduleDTO> GetTrainingScheduleById(long TrainingScheduleID);
        Task<dynamic> DeleteTrainingSchedule(TrainingScheduleDelete delete, string deletedbyUserEmail);
        Task<IEnumerable<TrainingScheduleDTO>> GetTrainingScheduleByCompany(long companyId);
        Task<string> ApproveTrainingSchedule(long TrainingScheduleID, long ApprovedByUserId);
        Task<string> DisaproveTrainingSchedule(long TrainingScheduleID, long DisapprovedByUserId, string DisapprovedComment);
    }
}
