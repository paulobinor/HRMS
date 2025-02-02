﻿using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;

namespace hrms_be_backend_business.ILogic
{
    public interface IResignationClearanceService
    {
        Task<ExecutedResult<string>> SubmitResignationClearance(ResignationClearanceVM payload, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<ResignationClearanceDTO>> GetResignationClearanceByID(long ID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<ResignationClearanceDTO>> GetResignationClearanceByEmployeeID(long UserID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<ResignationClearanceDTO>>> GetAllResignationClearanceByCompany(PaginationFilter filter, long companyID, string AccessKey, string RemoteIpAddress, DateTime? startDate, DateTime? endDate);

        Task<ExecutedResult<IEnumerable<ResignationClearanceDTO>>> GetPendingResignationClearanceByEmployeeID(long EmployeeID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<ResignationClearanceDTO>>> GetPendingResignationClearanceByCompanyID(long companyID, string AccessKey, string RemoteIpAddress, PaginationFilter filter, DateTime? startDate, DateTime? endDate);
        Task<ExecutedResult<string>> ApprovePendingResignationClearance(ApproveResignationClearanceDTO request, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<string>> DisapprovePendingResignationClearance(DisapprovePendingResignationClearanceDTO request, string AccessKey, string RemoteIpAddress);

    }
}
