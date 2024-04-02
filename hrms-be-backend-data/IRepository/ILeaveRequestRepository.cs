﻿using hrms_be_backend_common.Models;
using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface ILeaveRequestRepository
    {
        Task<string> CreateLeaveRequest(LeaveRequestCreate Leave);
        Task<LeaveRequestLineItem> CreateLeaveRequestLineItem(LeaveRequestLineItem leaveRequestLineItem);
        Task<LeaveRequestLineItem> RescheduleLeaveRequest(LeaveRequestLineItem leaveRequestLineItem);
        Task<dynamic> RescheduleLeaveRequest(RescheduleLeaveRequest update, string requesterUserEmail);
        Task<string> ApproveLeaveRequest(long LeaveRequestID, long ApprovedByUserId);
        Task<string> DisaproveLeaveRequest(long LeaveRequestID, long DisapprovedByUserId, string DisapprovedComment);
        Task<dynamic> DeleteLeaveRequest(LeaveRequestDelete delete, string deletedbyUserEmail);
        Task<IEnumerable<LeaveRequestDTO>> GetAllLeaveRequest();
        Task<LeaveRequestDTO> GetLeaveRequestById(long LeaveRequestID);
        Task<IEnumerable<LeaveRequestDTO>> GetLeaveRequestByUserId(long UserId, long CompanyId);
        Task<LeaveRequestDTO> GetLeaveRequestByYear(string RequestYear, long CompanyId);
        Task<IEnumerable<LeaveRequestDTO>> GetLeaveRequestByCompany(string RequestYear, long companyId);
        Task<IEnumerable<LeaveRequestDTO>> GetLeaveRequestPendingApproval(long UserIdGet);
        Task<List<EmpLeaveRequestInfo>> GetEmpLeaveInfo(long employeeId, string LeaveStatus = null, string companyId = null);
        Task<EmpLeaveRequestInfo> CreateEmpLeaveInfo(long employeeId);
        Task<LeaveRequestLineItem> GetLeaveRequestLineItem(long leaveRequestLineItemId);
        Task<LeaveApprovalInfo> GetLeaveApprovalInfo(long leaveApprovalId);
        Task<LeaveApprovalInfo> GetLeaveApprovalInfoByRequestLineItem(long leaveRequestLineitemId);
        Task<LeaveApprovalLineItem> GetLeaveApprovalLineItem(long leaveApprovalLineItemId, int approvalStep);
        Task<LeaveApprovalInfo> UpdateLeaveApprovalInfo(LeaveApprovalInfo leaveApproval);
        Task<LeaveApprovalLineItem> UpdateLeaveApprovalLineItem(LeaveApprovalLineItem leaveApprovalLineItem);
        Task<List<LeaveRequestLineItem>> GetLeaveRequestLineItems(long leaveRequestId);
        Task<GradeLeave> GradeLeave(long employeeId);
        Task<LeaveApprovalInfo> GetLeaveApprovalInfoByEmployeeId(long EmployeeId);
        Task<EmpLeaveRequestInfo> UpdateLeaveRequestInfoStatus(EmpLeaveRequestInfo empLeaveRequestInfo);
    }
}

