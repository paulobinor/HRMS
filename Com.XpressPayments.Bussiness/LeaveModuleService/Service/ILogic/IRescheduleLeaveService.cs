using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.LeaveModuleDTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.LeaveModuleService.Service.ILogic
{
    public interface IRescheduleLeaveService
    {

        Task<BaseResponse> CreateRescheduleLeaveRequest(RescheduleLeaveRequestCreate payload, RequesterInfo requester);
        Task<BaseResponse> ApproveRescheduleLeaveRequest(long RescheduleLeaveRequestID, RequesterInfo requester);
        Task<BaseResponse> DisaproveRescheduleLeaveRequest(RescheduleLeaveRequestDisapproved payload, RequesterInfo requester);
        Task<BaseResponse> GetAllRescheduleLeaveRquest(RequesterInfo requester);
        Task<BaseResponse> GetRescheduleLeaveRequsetById(long RescheduleLeaveRequestID, RequesterInfo requester);
        Task<BaseResponse> GetRescheduleLeaveRequestbyCompanyId(string RequestYear, long companyId, RequesterInfo requester);
        Task<BaseResponse> GetRescheduleLeaveRequestPendingApproval(RequesterInfo requester);
    }
}
