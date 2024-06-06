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
        public List<PendingLeaveApprovalItemsDto>? leaveRequestLineItems { get; set; }
    }
}
