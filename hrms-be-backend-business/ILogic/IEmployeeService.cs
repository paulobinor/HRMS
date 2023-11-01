﻿using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace hrms_be_backend_business.ILogic
{
    public interface IEmployeeService
    {
        Task<ExecutedResult<string>> CreateEmployeeBasis(CreateEmployeeBasisDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> UpdateEmployeePersonalInfo(UpdateEmployeePersonalInfoDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> UpdateEmployeeContactDetails(UpdateEmployeeContactDetailsDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);

        Task<ExecutedResult<string>> UpdateEmployeeBasis(UpdateEmployeeBasisDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> ApproveEmployee(long EmployeeId, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> DisapproveEmployee(long EmployeeId, string Comment, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> DeleteEmployee(long EmployeeId, string Comment, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<PagedExcutedResult<IEnumerable<EmployeeVm>>> GetEmployees(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<PagedExcutedResult<IEnumerable<EmployeeVm>>> GetEmployeesApproved(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<PagedExcutedResult<IEnumerable<EmployeeVm>>> GetEmployeesDisapproved(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<PagedExcutedResult<IEnumerable<EmployeeVm>>> GetEmployeesDeleted(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<EmployeeFullVm>> GetEmployeeById(long EmployeeId, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> CreateEmployeeBulkUpload(IFormFile payload, long companyID, RequesterInfo requester);
    }
}
