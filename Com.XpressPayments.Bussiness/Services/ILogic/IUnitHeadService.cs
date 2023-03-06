using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.GenericResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.ILogic
{
    public  interface IUnitHeadService
    {
        Task<BaseResponse> CreateUnitHead(CreateUnitHeadDTO creatDto, RequesterInfo requester);
        Task<BaseResponse> UpdateUnitHead(UpdateUnitHeadDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteUnitHead(DeleteUnitHeadDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveUnitHead(RequesterInfo requester);
        Task<BaseResponse> GetAllUnitHead(RequesterInfo requester);
        Task<BaseResponse> GetUnitHeadById(long UnitHeadID, RequesterInfo requester);
    }
}
