using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.GenericResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.ILogic
{
    public interface IEmployeeService
    {
        Task<BaseResponse> UpdateEmployee(UpdateEmployeeDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveEmployee(RequesterInfo requester);
        Task<BaseResponse> GetAllEmployee(RequesterInfo requester);
        Task<BaseResponse> GetEmployeeById(long EmpID, RequesterInfo requester);
        Task<BaseResponse> GetEmployeebyCompanyId(long companyId, RequesterInfo requester);
    }
}
