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
    }
}
