using Com.XpressPayments.Data.GenericResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.ILogic
{
    public interface ILgaService
    {
        Task<BaseResponse> GetAllLga(int StateID, RequesterInfo requester);
        Task<BaseResponse> GetLgaByStateId(int StateID, int LGAID, RequesterInfo requester);
    }
}
