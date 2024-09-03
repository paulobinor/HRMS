using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface IResignationInterviewService
    {
        Task<ExecutedResult<string>> SubmitResignationInterview( ResignationInterviewVM payload, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<ResignationInterviewDTO>> GetResignationInterviewById(long ResignationInterviewId, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<ResignationInterviewDTO>> GetResignationInterviewByEmployeeID(long EmployeeId, string AccessKey, string RemoteIpAddress);
        Task<BaseResponse> GetResignationInterviewDetails(long InterviewID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<ResignationInterviewDTO>>> GetAllResignationInterviewsByCompany(PaginationFilter filter, long companyID, string AccessKey, string RemoteIpAddress, DateTime? startDate, DateTime? endDate);
        Task<BaseResponse> GetInterviewScaleDetails(string AccessKey, string RemoteIpAddress);
        //Task<BaseResponse> ApprovePendingResignationInterview(ApproveResignationInterviewDTO request, RequesterInfo requester);
        //Task<BaseResponse> DisapprovePendingResignationInterview(DisapproveResignationInterviewDTO request, RequesterInfo requester);
    }
}
