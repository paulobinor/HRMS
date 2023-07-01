﻿using Com.XpressPayments.Data.LeaveModuleDTO.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.LeaveModuleRepository.LeaveRequestRepo
{
    public interface IRescheduleLeaveRepository
    {
        Task<string> CreateLeaveRequest(LeaveRequestCreate Leave);
        Task<string> ApproveLeaveRequest(long LeaveRequestID, long ApprovedByUserId);
        Task<string> DisaproveLeaveRequest(long LeaveRequestID, long DisapprovedByUserId, string DisapprovedComment);
        Task<dynamic> DeleteLeaveRequest(LeaveRequestDelete delete, string deletedbyUserEmail);
        Task<IEnumerable<LeaveRequestDTO>> GetAllLeaveRequest();
        Task<LeaveRequestDTO> GetLeaveRequestById(long LeaveRequestID);
        Task<LeaveRequestDTO> GetLeaveRequestByYear(string RequestYear);
        Task<LeaveRequestDTO> GetLeaveRequestByCompany(string RequestYear, long companyId);
        Task<IEnumerable<LeaveRequestDTO>> GetLeaveRequestPendingApproval(long UserIdGet);
    }
}
