using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.ViewModel;
using System.Security.Claims;

namespace hrms_be_backend_business.ILogic
{
    public  interface IEmploymentStatusService
    {
        Task<ExecutedResult<string>> CreateEmploymentStatus(CreateEmploymentStatusDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> UpdateEmploymentStatus(UpdateEmploymentStatusDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> DeleteEmploymentStatus(DeleteEmploymentStatusDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<PagedExcutedResult<IEnumerable<EmploymentStatusVm>>> GetEmploymentStatus(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<PagedExcutedResult<IEnumerable<EmploymentStatusVm>>> GetEmploymentStatusDeleted(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<EmploymentStatusVm>> GetEmploymentStatusById(long Id, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<EmploymentStatusVm>> GetEmploymentStatusByName(string EmploymentStatusName, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
    }
}
