using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.GenericResponse;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.ILogic
{
    public  interface  IEmployeeTypeService
    {
        Task<BaseResponse> CreateEmployeeType(CraeteEmployeeTypeDTO creatDto, RequesterInfo requester);
        Task<BaseResponse> CreateEmployeeTypeBulkUpload(IFormFile payload, RequesterInfo requester);
        Task<BaseResponse> UpdateEmployeeType(UpdateEmployeeTypeDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteEmployeeType(DeleteEmployeeTypeDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveEmployeeType(RequesterInfo requester);
        Task<BaseResponse> GetAllEmployeeType(RequesterInfo requester);
        Task<BaseResponse> GetEmployeeTypeById(long EmployeeTypeID, RequesterInfo requester);
        Task<BaseResponse> GetEmployeeTypebyCompanyId(long companyId, RequesterInfo requester);
    }
}
