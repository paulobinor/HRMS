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
        //Task<BaseResponse> CreateRescheduleLeaveRequest(RescheduleLeaveRequestCreateDTO payload, RequesterInfo requester);

        Task<BaseResponse> GetAllRescheduleLeaveRquest(RequesterInfo requester);
        Task<BaseResponse> GetRescheduleLeaveRequsetById(long RescheduleLeaveID, RequesterInfo requester);
        Task<BaseResponse> GetRescheduleLeaveRquestbyCompanyId(string RequestYear, long companyId, RequesterInfo requester);
    }
}
