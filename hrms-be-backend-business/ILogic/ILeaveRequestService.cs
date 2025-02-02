﻿using hrms_be_backend_common.DTO;
using hrms_be_backend_common.Models;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using System.Data;

namespace hrms_be_backend_business.ILogic
{
    public interface ILeaveRequestService
    {
        Task<BaseResponse> CreateLeaveRequestLineItem(LeaveRequestLineItem leaveRequestLineItem);
        Task<BaseResponse> CreateLeaveRequestLineItem(List<LeaveRequestLineItem> leaveRequestLineItems);
        // Task<BaseResponse> RescheduleLeaveRequest(RescheduleLeaveRequest updateDto, RequesterInfo requester);
        Task<BaseResponse> RescheduleLeaveRequest(LeaveRequestLineItem leaveRequestLineItem);
        //  Task<BaseResponse> UpdateLeaveApproveLineItem(LeaveApprovalLineItem leaveApprovalLineItem);
        Task<BaseResponse> GetLeaveRequestLineItem(long leaveRequestLineItemId);
        Task<BaseResponse> GetEmployCumulativeForLeaveType(LeaveRequestLineItem leaveRequestLineItem);
        //  Task<LeaveApprovalInfo> GetLeaveApprovalInfo(long leaveApprovalId, long leaveReqestLineItemId);
        Task<EmpLeaveRequestInfo> GetEmpLeaveInfo(long employeeId, long companyId, string year);
        Task<List<AnnualLeave>> GetEmpAnnualLeaveInfoList(long employeeId, long companyId);
        Task<AnnualLeave> GetEmpAnnualLeaveInfo(int AnnualLeaveId);
        Task<AnnualLeave> GetEmpAnnualLeaveInfo(int EmployeeId, int CompanyId, int LeavePeriod);
        Task<AnnualLeave> CheckAnnualLeaveInfo(LeaveRequestLineItem leaveRequestLineItem);

        Task<BaseResponse> GetAllLeaveRequest(string CompanyID, DateTime? startDate, DateTime? endDate, string ApprovalPosition = "All", string approvalStatus = "All", int pageNumber = 1, int pageSize = 10);
        Task<BaseResponse> GetAllLeaveRequest(string CompanyID);

        Task<BaseResponse> GetAnnualLeaveRequests(long CompanyID, DateTime? startdate, DateTime? endDate, string ApprovalPosition = "All", string approvalStatus = "All", int pageNumber = 1, int pageSize = 10, string year = null);

        Task<BaseResponse> GetAnnualLeaveRequests(long CompanyID, string year = null);
        Task<BaseResponse> GetAllLeaveRquestLineItems(long CompanyID);
        Task<List<LeaveRequestLineItemDto>> GetEmployeeLeaveRequests(long CompanyID, long EmployeeId);
        Task<List<LeaveRequestLineItemDto>> GetEmployeeLeaveRequests(long CompanyID, long EmployeeId, DateTime? startDate, DateTime? endDate, int pageNumber = 1, int pageSize = 10);
        Task<BaseResponse> RescheduleAnnualLeaveRequest(List<LeaveRequestLineItem> leaveRequestLineItems);
        Task<BaseResponse> GetEmpAnnualLeaveRquestLineItems(long v);
        //Task<List<LeaveApprovalLineItem>> GetAllLeaveApprovalLineItems(string companyID);

        #region Depricated
        //Task<BaseResponse> GetLeaveRequsetById(long LeaveRequestID, RequesterInfo requester);
        //Task<BaseResponse> GetLeaveRequsetByUerId(long UserId, long CompanyId, RequesterInfo requester);
        //Task<BaseResponse> GetLeaveRquestbyCompanyId(string RequestYear, long companyId, RequesterInfo requester);
        //Task<BaseResponse> GetLeaveRequestPendingApproval(RequesterInfo requester);
        //Task<BaseResponse> CreateLeaveRequest(LeaveRequestCreate payload, RequesterInfo requester);
        //Task<BaseResponse> ApproveLeaveRequest(long LeaveRequestID, RequesterInfo requester);
        //Task<BaseResponse> DisaproveLeaveRequest(LeaveRequestDisapproved payload, RequesterInfo requester); 
        #endregion

    }
}
