using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.GenericResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.ILogic
{
    public interface IHospitalProvidersService
    {
        Task<BaseResponse> CreateHospitalProviders(CreateHospitalProvidersDTO create, RequesterInfo requester);
        Task<BaseResponse> UpdateHospitalProviders(UpdateHospitalProvidersDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteHospitalProviders(DeleteHospitalProvidersDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveHospitalProviders(RequesterInfo requester);
        Task<BaseResponse> GetAllHospitalProviders(RequesterInfo requester);
        Task<BaseResponse> GetHospitalProvidersbyId(long ID, RequesterInfo requester);
        Task<BaseResponse> GetHospitalProvidersbyCompanyId(long companyId, RequesterInfo requester);

    }
}
