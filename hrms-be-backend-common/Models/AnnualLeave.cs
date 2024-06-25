using hrms_be_backend_common.DTO;

namespace hrms_be_backend_common.Models
{
    public class AnnualLeave
    {
        public int AnnualLeaveId { get; set; }
        public int CompanyID { get; set; }
        public int EmployeeId { get; set; }
        public int LeaveRequestId { get; set; }
        public DateTime DateCreated { get; set; }
        public int TotalNoOfDays { get; set; }
        public int totalDays => TotalNoOfDays;
        public int NoOfDaysTaken { get; set; }
        public int LeavePeriod { get; set; }
        public byte IsApproved { get; set; }
        public int SplitCount { get; set; }
        public string ApprovalStatus { get; set; }
        public string Comments { get; set; }
        public List<LeaveRequestLineItemDto>? leaveRequestLineItems { get; set; }
    }
}
