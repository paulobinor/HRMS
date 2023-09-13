using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.LearningAndDevelopmentModuleService.Service.ILogic
{
    public interface ITrainingFeedbackFormService
    {
        Task<BaseResponse> CreateTraineeTrainingFeedbackForm(TraineeTrainingFeedbackFormCreate payload, RequesterInfo requester);
        Task<BaseResponse> CreateSupervisorTrainingFeedbackForm(SupervisorTrainingFeedbackFormCreate payload, RequesterInfo requester);
    }
}
