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
        Task<BaseResponse> GetAllState(int CountryID, RequesterInfo requester);
        Task<BaseResponse> GetStateByCountryId(int CountryID, int StateID, RequesterInfo requester);
    }
}
