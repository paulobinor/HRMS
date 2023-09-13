using Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.LearningAndDevelopmentRepository.TrainingFeedbackFormRepo
{
    public interface ITrainingFeedbackFormRepository
    {
        Task<string> CreateTraineeTrainingFeedbackForm(TraineeTrainingFeedbackFormCreate traineeTrainingFeedbackForm, string createdbyUserEmail);
        Task<string> CreateSupervisorTrainingFeedbackForm(SupervisorTrainingFeedbackFormCreate supervisorTrainingFeedbackForm, string createdbyUserEmail);
    }
}
