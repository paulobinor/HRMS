using hrms_be_backend_common.Models;

namespace hrms_be_backend_common.DTO
{
    public class PendingAnnualLeaveApprovalItemDto
    {
        public string FullName { get; set; }
        public string LeaveTypeName { get; set; }
        public string Year { get; set; }
        public int TotalNoOfDays { get; set; }
        public int LeaveCount { get; set; }
        public string Status { get; set; }
        public long EmployeeID { get; set; }
        //  public DateTime ResumptionDate { get; set; }
        public int LeaveLength { get; set; }
        public string ApprovalStatus { get; set; }
        public string RelieverName { get; set; }
        public List<LeaveRequestLineItemDto>? leaveRequestLineItems { get; set; }
        public long leaveApprovalId { get; set; }
        public long LastApprovalEmployeeId { get; set; }
        public string ApprovalPosition { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
