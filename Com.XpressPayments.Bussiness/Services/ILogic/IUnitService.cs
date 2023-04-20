using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.GenericResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.ILogic
{
    public  interface IUnitService
    {
        Task<BaseResponse> CreateUnit(CreateUnitDTO unitDto, RequesterInfo requester);
        Task<BaseResponse> UpdateUnit(UpdateUnitDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteUnit(DeleteUnitDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveUnit(RequesterInfo requester);
        Task<BaseResponse> GetAllUnit(RequesterInfo requester);
        Task<BaseResponse> GetUnitById(long UnitID, RequesterInfo requester);
        Task<BaseResponse> GetUnitbyCompanyId(long companyId, RequesterInfo requester);
    }
}
