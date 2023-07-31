using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.OnBoardingDTO.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.OnBoardingModuleService.Services.ILogic
{
    public interface IReviwerService
    {
        Task<BaseResponse> CreateReviwer(CreateReviwerDTO create, RequesterInfo requester);
        Task<BaseResponse> DeleteReviwer(DeleteReviwerDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetReviwerbyId(long UserId, RequesterInfo requester);
        Task<BaseResponse> GetReviwerbyCompanyId(long companyId, RequesterInfo requester);
    }
}
