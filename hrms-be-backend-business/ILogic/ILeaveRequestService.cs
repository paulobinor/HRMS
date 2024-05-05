using hrms_be_backend_common.DTO;
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
       // Task<BaseResponse> RescheduleLeaveRequest(RescheduleLeaveRequest updateDto, RequesterInfo requester);
        Task<BaseResponse> RescheduleLeaveRequest(LeaveRequestLineItem leaveRequestLineItem);
      //  Task<BaseResponse> UpdateLeaveApproveLineItem(LeaveApprovalLineItem leaveApprovalLineItem);
        Task<BaseResponse> GetLeaveRequestLineItem(long leaveRequestLineItemId);
      //  Task<LeaveApprovalInfo> GetLeaveApprovalInfo(long leaveApprovalId, long leaveReqestLineItemId);
        Task<EmpLeaveRequestInfo> GetEmpLeaveInfo(long employeeId, long companyId, string LeaveStatus = "Active");
       // Task<LeaveApprovalInfo> UpdateLeaveApprovalInfo(LeaveApprovalInfo leaveApproval);
      //  Task<LeaveApprovalLineItem> GetLeaveApprovalLineItem(long leaveApprovalLineItemId, int approvalStep = 0);
      //  Task<LeaveApprovalInfo> GetLeaveApprovalInfoByRequestLineItemId(long leaveRequestLineItemId);
       // Task<List<LeaveApprovalLineItem>>GetleaveApprovalLineItems(long leaveApprovalId);
        Task<BaseResponse> GetAllLeaveRequest(string CompanyID);
        Task<BaseResponse> GetAllLeaveRquestLineItems(long CompanyID);
        Task<List<LeaveRequestLineItemDto>> GetEmployeeLeaveRequests(long CompanyID, long EmployeeId);
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
