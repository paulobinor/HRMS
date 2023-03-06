using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.GenericResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.ILogic
{
    public  interface IHODService
    {
        Task<BaseResponse> CreateHOD(CreateHodDTO HodDto, RequesterInfo requester);
        Task<BaseResponse> UpdateHOD(UpdateHodDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteHOD(DeleteHodDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveHOD(RequesterInfo requester);
        Task<BaseResponse> GetAllHOD(RequesterInfo requester);
        Task<BaseResponse> GetHODbyId(long HodID, RequesterInfo requester);

    }
}
