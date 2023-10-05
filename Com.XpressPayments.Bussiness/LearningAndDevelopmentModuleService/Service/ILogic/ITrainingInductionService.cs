using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.LearningAndDevelopmentModuleService.Service.ILogic
{
    public interface ITrainingInductionService
    {
        Task<BaseResponse> CreateTrainingInduction(TrainingInductionCreate payload, RequesterInfo requester);
        Task<BaseResponse> UpdateTrainingInduction(TrainingInductionUpdate payload, RequesterInfo requester);
        Task<BaseResponse> DeleteTrainingInduction(TrainingInductionDelete payload, RequesterInfo requester);
        Task<BaseResponse> ApproveTrainingInduction(TrainingInductionApproved payload, RequesterInfo requester);
        Task<BaseResponse> DisaproveTrainingInduction(TrainingInductionDisapproved payload, RequesterInfo requester);
        Task<BaseResponse> GetAllTrainingInduction(RequesterInfo requester);
        Task<BaseResponse> GetAllActiveTrainingInduction(RequesterInfo requester);
        Task<BaseResponse> GetTrainingInductionById(long TrainingInductionID, RequesterInfo requester);
        Task<BaseResponse> GetTrainingInductionPendingApproval(RequesterInfo requester);
        Task<BaseResponse> GetTrainingInductionbyCompanyId(long companyId, RequesterInfo requester);
    }
}
