using Com.XpressPayments.Data.LeaveModuleDTO.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.LeaveModuleRepository.LeaveRequestRepo
{
    public interface IRescheduleLeaveRepository
    {
        Task<string> CreateRescheduleLeaveRequest(RescheduleLeaveRequestCreate Leave);
        Task<string> ApproveRescheduleLeaveRequest(long RescheduleLeaveID, long ApprovedByUserId);
        Task<string> DisaproveRescheduleLeaveRequest(long RescheduleLeaveID, long DisapprovedByUserId, string DisapprovedComment);
        Task<IEnumerable<RescheduleLeaveRequestDTO>> GetAllRescheduleLeaveRequest();
        Task<RescheduleLeaveRequestDTO> GetRescheduleLeaveRequestById(long RescheduleLeaveID);
        Task<RescheduleLeaveRequestDTO> GetRescheduleLeaveRequestByYear(string RequestYear);
        Task<RescheduleLeaveRequestDTO> GetRescheduleLeaveRequestByCompanyId(string RequestYear, long companyId);
        Task<IEnumerable<RescheduleLeaveRequestDTO>> GetRescheduleLeaveRequestPendingApproval(long UserIdGet);
    }
}
