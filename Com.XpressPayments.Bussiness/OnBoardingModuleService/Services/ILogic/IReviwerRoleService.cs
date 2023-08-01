using Com.XpressPayments.Data.GenericResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.OnBoardingModuleService.Services.ILogic
{
    public interface IReviwerRoleService
    {
        Task<BaseResponse> GetReviwerRolebyId(long ReviwerRoleID, RequesterInfo requester);
        Task<BaseResponse> GetReviwerRolebyCompanyId(long companyId, RequesterInfo requester);
    }
}
