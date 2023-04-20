using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.GenericResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.ILogic
{
    public interface IEmpLocationService
    {
        Task<BaseResponse> CreateEmpLocation(CreateEmpLocationDTO creatDto, RequesterInfo requester);
        Task<BaseResponse> UpdateEmpLocation(UpdateEmpLocationDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteEmpLocation(DeleteEmpLocationDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveEmpLocation(RequesterInfo requester);
        Task<BaseResponse> GetAllEmpLocation(RequesterInfo requester);
        Task<BaseResponse> GetEmpLocationbyId(long EmpLocationID, RequesterInfo requester);
        Task<BaseResponse> GetEmpLocationbyCompanyId(long CompanyID, RequesterInfo requester);
    }
}
