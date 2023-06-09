using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.GenericResponse;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.ILogic
{
    public interface IDepartmentService
    {
        Task<BaseResponse> CreateDepartment(CreateDepartmentDto DepartmentDto, RequesterInfo requester);
        Task<BaseResponse> CreateDepartmentBulkUpload(IFormFile payload, RequesterInfo requester);
        Task<BaseResponse> UpdateDepartment(UpdateDepartmentDto updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteDepartment(DeleteDepartmentDto deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveDepartments(RequesterInfo requester);
        Task<BaseResponse> GetAllDepartments(RequesterInfo requester);
        Task<BaseResponse> GetDepartmentbyId(long DepartmentId, RequesterInfo requester);
        Task<BaseResponse> GetDepartmentbyCompanyId(long companyId, RequesterInfo requester);
    }

   
}
