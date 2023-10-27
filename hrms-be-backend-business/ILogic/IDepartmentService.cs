using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace hrms_be_backend_business.ILogic
{
    public interface IDepartmentService
    {
        Task<ExecutedResult<string>> CreateDepartment(CreateDepartmentDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> CreateDepartmentBulkUpload(IFormFile payload, string AccessKey, IEnumerable<Claim> claim, RequesterInfo requester);
        Task<ExecutedResult<string>> UpdateDepartment(UpdateDepartmentDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> DeleteDepartment(DeleteDepartmentDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<PagedExcutedResult<IEnumerable<DepartmentVm>>> GetDepartmentes(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<PagedExcutedResult<IEnumerable<DepartmentVm>>> GetDepartmentesDeleted(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<DepartmentVm>> GetDepartmentById(long Id, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<DepartmentVm>> GetDepartmentByName(string DepartmentName, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
    }

   
}
