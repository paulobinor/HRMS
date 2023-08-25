using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.LearningAndDevelopmentModuleService.Service.ILogic
{
    public interface ITrainingScheduleService
    {
        Task<BaseResponse> CreateTrainingSchedule(TrainingScheduleCreate payload, RequesterInfo requester);
        Task<BaseResponse> GetAllTrainingSchedule(RequesterInfo requester);
        Task<BaseResponse> GetTrainingScheduleById(long TrainingScheduleID, RequesterInfo requester);
        Task<BaseResponse> GetTrainingSchedulebyCompanyId(long companyId, RequesterInfo requester);
        Task<BaseResponse> ApproveTrainingSchedule(long TrainingPlanID, RequesterInfo requester);
        Task<BaseResponse> DisaproveTrainingSchedule(TrainingScheduleDisapproved payload, RequesterInfo requester);
    }
}
