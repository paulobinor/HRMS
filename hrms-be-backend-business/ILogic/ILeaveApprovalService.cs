using hrms_be_backend_common.DTO;
using hrms_be_backend_common.Models;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using System.Data;

namespace hrms_be_backend_business.ILogic
{
    public interface ILeaveApprovalService
    {
        Task<BaseResponse> UpdateLeaveApproveLineItem(LeaveApprovalLineItem leaveApprovalLineItem);

        Task<LeaveApprovalInfo> GetAnnualLeaveApprovalInfo(long leaveApprovalId, long leaveReqestLineItemId);
        Task<LeaveApprovalInfo> GetLeaveApprovalInfo(long leaveApprovalId, long leaveReqestLineItemId);
        Task<LeaveApprovalInfo> UpdateLeaveApprovalInfo(LeaveApprovalInfo leaveApproval);
        Task<LeaveApprovalLineItem> GetLeaveApprovalLineItem(long leaveApprovalLineItemId, int approvalStep = 0);
        Task<LeaveApprovalInfo> GetLeaveApprovalInfoByRequestLineItemId(long leaveRequestLineItemId);
        Task<List<LeaveApprovalLineItem>>GetleaveApprovalLineItems(long leaveApprovalId);
        Task<List<LeaveApprovalInfoDto>> GetLeaveApprovalInfoByCompanyID(long companyID);
        Task<List<PendingLeaveApprovalItemsDto>> GetPendingLeaveApprovals(long approvalEmployeeID, string v = null);
        Task<List<PendingLeaveApprovalItemsDto>> GetPendingAnnualLeaveApprovals(long approvalEmployeeID, string v = null);
        Task<BaseResponse> UpdateLeaveApproveLineItems(List<LeaveApprovalLineItem> leaveApprovalLineItems, string approvalStatus);

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
