using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.ViewModel;
using System.Security.Claims;

namespace hrms_be_backend_business.ILogic
{
    public interface IUserService
    {
        //Task<ExecutedResult<string>> CreateCompanyUser(CreateUserDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> CreateBackOfficeUser(CreateUserDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> UpdateBackOfficeUser(UpdateUserDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> ApproveUserByBackOffice(long UserId, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> DeactivateUserBackOffice(DeactivateUserDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> DisapproveUserBackOffice(DisapproveUserDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);

        Task<PagedExcutedResult<IEnumerable<UserVm>>> GetUsers(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<PagedExcutedResult<IEnumerable<UserVm>>> GetUsersBackOffice(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<PagedExcutedResult<IEnumerable<UserVm>>> GetUsersApprovedBackOffice(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<PagedExcutedResult<IEnumerable<UserVm>>> GetUsersDisapprovedBackOffice(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<PagedExcutedResult<IEnumerable<UserVm>>> GetUsersDeapctivatedBackOffice(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<PagedExcutedResult<IEnumerable<UserVm>>> GetUsersByCompany(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<PagedExcutedResult<IEnumerable<UserVm>>> GetUsersApprovedByCompany(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<PagedExcutedResult<IEnumerable<UserVm>>> GetUsersDisapprovedByCompany(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<PagedExcutedResult<IEnumerable<UserVm>>> GetUsersDeactivatedByCompany(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<UserVm>> GetUserById(long UserId, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
    }
}
