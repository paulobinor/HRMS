using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs;
using Com.XpressPayments.Data.LeaveModuleDTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.LearningAndDevelopmentModuleService.Service.ILogic
{
    internal interface ITrainingPlanService
    {
        Task<BaseResponse> CreateTrainingPlan(TrainingPlanCreate payload, RequesterInfo requester);
        Task<BaseResponse> ApproveTrainingPlan(long TrainingPlanID, RequesterInfo requester);
        Task<BaseResponse> DisaproveTrainingPlan(TrainingPlanDisapproved payload, RequesterInfo requester);
        Task<BaseResponse> GetAllTrainingPlan(RequesterInfo requester);
        Task<BaseResponse> GetTrainingPlanByUserId(long UserID, RequesterInfo requester);
        Task<BaseResponse> GetTrainingPlanById(long TrainingPlanID, RequesterInfo requester);
    }
}
