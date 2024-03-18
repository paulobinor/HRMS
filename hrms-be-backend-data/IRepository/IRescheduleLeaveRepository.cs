using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface IRescheduleLeaveRepository
    {
        Task<string> CreateRescheduleLeaveRequest(RescheduleLeaveRequestCreate Leave);
        Task<string> ApproveRescheduleLeaveRequest(long RescheduleLeaveID, long ApprovedByUserId);
        Task<string> DisaproveRescheduleLeaveRequest(long RescheduleLeaveID, long DisapprovedByUserId, string DisapprovedComment);
        Task<IEnumerable<RescheduleLeaveRequestDTO>> GetAllRescheduleLeaveRequest();
        Task<RescheduleLeaveRequestDTO> GetRescheduleLeaveRequestById(long RescheduleLeaveID);
        Task<IEnumerable<RescheduleLeaveRequestDTO>> GetRescheduleLeaveRequestByUserId(long UserId, long CompanyId);
        Task<RescheduleLeaveRequestDTO> GetRescheduleLeaveRequestByYear(string RequestYear);
        Task<RescheduleLeaveRequestDTO> GetRescheduleLeaveRequestByCompanyId(string RequestYear, long companyId);
        Task<IEnumerable<RescheduleLeaveRequestDTO>> GetRescheduleLeaveRequestPendingApproval(long UserIdGet);
    }
}
