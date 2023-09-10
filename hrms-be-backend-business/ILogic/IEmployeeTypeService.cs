using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;

namespace hrms_be_backend_business.ILogic
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
