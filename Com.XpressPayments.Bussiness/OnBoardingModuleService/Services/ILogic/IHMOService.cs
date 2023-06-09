using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.GenericResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.ILogic
{
    public interface IHMOService
    {
        Task<BaseResponse> CreateHMO(CreateHMODTO HomDto, RequesterInfo requester);
        Task<BaseResponse> UpdateHMO(UpdateHMODTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteHMO(DeleteHMODTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveHMO(RequesterInfo requester);
        Task<BaseResponse> GetAllHMO(RequesterInfo requester);
        Task<BaseResponse> GetHMObyId(long ID, RequesterInfo requester);
        Task<BaseResponse> GetHMObyCompanyId(long companyId, RequesterInfo requester);
    }
}
