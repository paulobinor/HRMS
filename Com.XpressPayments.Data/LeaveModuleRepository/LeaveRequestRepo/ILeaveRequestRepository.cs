using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.LeaveModuleDTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.LeaveModuleRepository.LeaveRequestRepo
{
    public  interface ILeaveRequestRepository
    {
        Task<dynamic> CreateLeaveRequest(LeaveRequestCreate Leave, string createdbyUserEmail);
        Task<dynamic> DeleteLeaveRequest(LeaveRequestDelete delete, string deletedbyUserEmail);
        Task<IEnumerable<LeaveRequestDTO>> GetAllLeaveRequest();
        Task<LeaveRequestDTO> GetLeaveRequestById(long LeaveRequestID);
        Task<DepartmentsDTO> GetLeaveRequestByName(string RequestYear);
        Task<LeaveRequestDTO> GetLeaveRequestByCompany(string RequestYear, int companyId);
    }
}
