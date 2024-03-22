using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface ILeaveRequestService
    {
        Task<BaseResponse> CreateLeaveRequest(LeaveRequestCreate payload, RequesterInfo requester);
        Task<BaseResponse> CreateLeaveRequest(LeaveRequestLineItem leaveRequestLineItem);
        Task<BaseResponse> RescheduleLeaveRequest(RescheduleLeaveRequest updateDto, RequesterInfo requester);
        Task<BaseResponse> RescheduleLeaveRequest(LeaveRequestLineItem leaveRequestLineItem);
        Task<BaseResponse> ApproveLeaveRequest(long LeaveRequestID, RequesterInfo requester);
        Task<BaseResponse> DisaproveLeaveRequest(LeaveRequestDisapproved payload, RequesterInfo requester);
        Task<BaseResponse> GetAllLeaveRquest(RequesterInfo requester);
        Task<BaseResponse> GetLeaveRequsetById(long LeaveRequestID, RequesterInfo requester);
        Task<BaseResponse> GetLeaveRequsetByUerId(long UserId, long CompanyId,  RequesterInfo requester);
        Task<BaseResponse> GetLeaveRquestbyCompanyId(string RequestYear, long companyId, RequesterInfo requester);
        Task<BaseResponse> GetLeaveRequestPendingApproval(  RequesterInfo requester );
    }
}
