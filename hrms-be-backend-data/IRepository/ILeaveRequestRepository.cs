﻿using hrms_be_backend_common.DTO;
using hrms_be_backend_common.Models;
using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface ILeaveRequestRepository
    {
        Task<string> CreateLeaveRequest(LeaveRequestCreate Leave);
        Task<LeaveRequestLineItem> CreateLeaveRequestLineItem(LeaveRequestLineItem leaveRequestLineItem);
        Task<LeaveRequestLineItem> RescheduleLeaveRequest(LeaveRequestLineItem leaveRequestLineItem);
        Task<string> RescheduleAnnualLeaveRequest(List<LeaveRequestLineItem> leaveRequestLineItems, AnnualLeave annualLeave);
        Task<dynamic> RescheduleLeaveRequest(RescheduleLeaveRequest update, string requesterUserEmail);
        Task<string> ApproveLeaveRequest(long LeaveRequestID, long ApprovedByUserId);
        Task<string> DisaproveLeaveRequest(long LeaveRequestID, long DisapprovedByUserId, string DisapprovedComment);
        Task<dynamic> DeleteLeaveRequest(LeaveRequestDelete delete, string deletedbyUserEmail);
        Task<IEnumerable<EmpLeaveRequestInfo>> GetAllLeaveRequest(string CompanyId);
        Task<LeaveRequestDTO> GetLeaveRequestById(long LeaveRequestID);
        Task<IEnumerable<LeaveRequestDTO>> GetLeaveRequestByUserId(long UserId, long CompanyId);
        Task<LeaveRequestDTO> GetLeaveRequestByYear(string RequestYear, long CompanyId);
        Task<IEnumerable<LeaveRequestDTO>> GetLeaveRequestByCompany(string RequestYear, long companyId);
        Task<IEnumerable<LeaveRequestDTO>> GetLeaveRequestPendingApproval(long UserIdGet);
        Task<EmpLeaveRequestInfo> GetEmpLeaveInfo(long employeeId, long companyId, string year);
        Task<EmpLeaveRequestInfo> CreateEmpLeaveInfo(long employeeId, long companyId);


        Task<LeaveRequestLineItem> GetLeaveRequestLineItem(long leaveRequestLineItemId);
     //   Task<LeaveRequestLineItem> GetAnnualLeaveRequestLineItem(long leaveApprovalId);
        Task<LeaveApprovalInfo> GetLeaveApprovalInfo(long leaveApprovalId);
        Task<LeaveApprovalInfo> GetLeaveApprovalInfoByRequestLineItemId(long leaveRequestLineitemId);
       // Task<LeaveApprovalLineItem> GetLeaveApprovalLineItem(long leaveApprovalLineItemId, int approvalStep);
       // Task<LeaveApprovalInfo> UpdateLeaveApprovalInfo(LeaveApprovalInfo leaveApproval);
      //  Task<LeaveApprovalLineItemDto> UpdateLeaveApprovalLineItem(LeaveApprovalLineItemDto leaveApprovalLineItem);
        Task<List<LeaveRequestLineItem>> GetLeaveRequestLineItems(long leaveRequestId);
        Task<List<LeaveRequestLineItemDto>> GetAllLeaveRequestLineItems(long CompanyID);
        Task<List<LeaveRequestLineItemDto>> GetAllAnnualLeaveRequestLineItems(long CompanyID);
        Task<List<LeaveRequestLineItemDto>> GetAllAnnualLeaveRequestLineItems(int AnnualLeaveId, string CompanyId = null);
        Task<List<AnnualLeaveDto>> GetAllAnnualLeaveRequests(long CompanyID, string leavePeriod);
        Task<GradeLeave> GetEmployeeGradeLeave(long employeeId, long leaveTypeId);
       // Task<LeaveApprovalInfo> GetLeaveApprovalInfoByEmployeeId(long EmployeeId);
        Task<EmpLeaveRequestInfo> UpdateLeaveRequestInfoStatus(EmpLeaveRequestInfo empLeaveRequestInfo);
       // Task<List<LeaveApprovalLineItemDto>> GetLeaveApprovalLineItems(long leaveApprovalId);
        Task UpdateLeaveRequestLineItemApproval(LeaveRequestLineItem leaveRequestLineItem);
        Task<List<LeaveRequestLineItemDto>> GetEmployeeLeaveRequests(long companyID, long employeeID);

        //Annual Leave
        //  Task<EmpLeaveRequestInfo> CreateAnnualLeaveInfo(long employeeId, string CompanyID);
        Task<List<AnnualLeave>> GetAnnualLeaveInfo(int employeeId, int companyId);
        Task<AnnualLeave> GetAnnualLeaveInfo(int AnnualLeaveId);
        Task<AnnualLeave> CheckAnnualLeaveInfo(LeaveRequestLineItem leaveRequestLineItem);
        Task<AnnualLeave> CreateAnnualLeaveRequest(AnnualLeave annualLeave, List<LeaveRequestLineItem> requestLineItems);
        Task<AnnualLeave> CreateAnnualLeaveRequest(List<LeaveRequestLineItem> requestLineItems);
        Task<AnnualLeave> UpdateAnnualLeave(AnnualLeave anualLeave);
        Task<LeaveApprovalLineItem> UpdateLeaveRequestApprovalID(LeaveRequestLineItem leaveRequestLineItem);
        Task<AnnualLeave> GetAnnualLeaveInfo(int employeeId, int companyId, int leavePeriod);
        Task<LeaveRequestLineItem> GetLeaveRequest(long LeaveRequestId, long LeaveTypeId);

        //  Task<EmpLeaveRequestInfo> UpdateAnnualLeaveInfo(long employeeId, string CompanyID);

    }
}

