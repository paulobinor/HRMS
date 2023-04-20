using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.GenericResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.ILogic
{
    public  interface IEmploymentStatusService
    {
        Task<BaseResponse> CreateEmploymentStatus(CreateEmploymentStatusDTO creatDto, RequesterInfo requester);
        Task<BaseResponse> UpdateEmploymentStatus(UpdateEmploymentStatusDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteEmploymentStatus(DeleteEmploymentStatusDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveEmploymentStatus(RequesterInfo requester);
        Task<BaseResponse> GetAllEmploymentStatus(RequesterInfo requester);
        Task<BaseResponse> GetEmploymentStatusbyId(long EmploymentStatusID, RequesterInfo requester);
        Task<BaseResponse> GetEmpLoymentStatusbyCompanyId(long CompanyID, RequesterInfo requester);
    }
}
