using Com.XpressPayments.Data.GenericResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.ILogic
{
    public interface IStateService
    {
        Task<BaseResponse> GetAllState(long CountryID, RequesterInfo requester);
        Task<BaseResponse> GetStateByCountryId(long CountryID, RequesterInfo requester);
    }
}
