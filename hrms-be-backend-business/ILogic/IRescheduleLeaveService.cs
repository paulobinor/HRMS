using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface IRescheduleLeaveService
    {

        Task<BaseResponse> CreateRescheduleLeaveRequest(RescheduleLeaveRequestCreate payload, RequesterInfo requester);
        Task<BaseResponse> ApproveRescheduleLeaveRequest(long RescheduleLeaveRequestID, RequesterInfo requester);
        Task<BaseResponse> DisaproveRescheduleLeaveRequest(RescheduleLeaveRequestDisapproved payload, RequesterInfo requester);
        Task<BaseResponse> GetAllRescheduleLeaveRquest(RequesterInfo requester);
        Task<BaseResponse> GetRescheduleLeaveRequsetById(long RescheduleLeaveRequestID,  RequesterInfo requester);
        Task<BaseResponse> GetRescheduleLeaveRequsetByUserId(long UserId, long CompanyId, RequesterInfo requester);
        Task<BaseResponse> GetRescheduleLeaveRequestbyCompanyId(string RequestYear, long companyId, RequesterInfo requester);
        Task<BaseResponse> GetRescheduleLeaveRequestPendingApproval(RequesterInfo requester);
    }
}
