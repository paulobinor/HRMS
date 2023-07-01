using Com.XpressPayments.Data.LeaveModuleDTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.LeaveModuleRepository.LeaveRequestRepo
{
    public interface IRescheduleLeaveRepository
    {
        Task<dynamic> CreateRescheduleLeave(RescheduleLeaveRequestCreateDTO RescheduleLeave, string createdbyUserEmail);
        Task<string> ApproveRescheduleLeave(long RescheduleLeaveID, long ApprovedByUserId);
        Task<string> DisaproveRescheduleLeave(long RescheduleLeaveID, long DisapprovedByUserId, string DisapprovedComment);
        Task<IEnumerable<RescheduleLeaveRequestDTO>> GetAllRescheduleLeave();
        Task<RescheduleLeaveRequestDTO> GetRescheduleLeaveById(long RescheduleLeaveID);
        Task<RescheduleLeaveRequestDTO> GetRescheduleLeaveRequestByYear(string RequestYear);
        Task<RescheduleLeaveRequestDTO> GetRescheduleLeaveRequestByCompany(string RequestYear, long companyId);
    }
}
