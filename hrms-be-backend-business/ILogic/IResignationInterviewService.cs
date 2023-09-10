using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface IResignationInterviewService
    {
        Task<BaseResponse> SubmitResignationInterview(RequesterInfo requesterInfo, ResignationInterviewVM payload);
        Task<BaseResponse> GetInterviewScaleDetails(RequesterInfo requester);
        Task<BaseResponse> GetResignationInterview(long SRFID, RequesterInfo requester);
        Task<BaseResponse> GetResignationInterviewDetails(long InterviewID, RequesterInfo requester);
        Task<BaseResponse> ApprovePendingResignationInterview(ApproveResignationInterviewDTO request, RequesterInfo requester);
        Task<BaseResponse> DisapprovePendingResignationInterview(DisapproveResignationInterviewDTO request, RequesterInfo requester);
        Task<BaseResponse> GetAllApprovedResignationInterview(long UserID, bool isApproved, RequesterInfo requester);
    }
}
