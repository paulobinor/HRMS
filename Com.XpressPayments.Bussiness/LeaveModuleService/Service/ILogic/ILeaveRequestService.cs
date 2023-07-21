using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.LeaveModuleDTO.DTO;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.LeaveModuleService.Service.ILogic
{
    public interface ILeaveRequestService
    {
        Task<BaseResponse> CreateLeaveRequest(LeaveRequestCreate payload, RequesterInfo requester);
        Task<BaseResponse> ApproveLeaveRequest(long LeaveRequestID, RequesterInfo requester);
        Task<BaseResponse> DisaproveLeaveRequest(LeaveRequestDisapproved payload, RequesterInfo requester);
        Task<BaseResponse> GetAllLeaveRquest(RequesterInfo requester);
        Task<BaseResponse> GetLeaveRequsetById(long LeaveRequestID, RequesterInfo requester);
        Task<BaseResponse> GetLeaveRquestbyCompanyId(string RequestYear, long companyId, RequesterInfo requester);
        Task<BaseResponse> GetLeaveRequestPendingApproval(  RequesterInfo requester );
    }
}
