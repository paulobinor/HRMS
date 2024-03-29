namespace hrms_be_backend_common.Models
{
    public class LeaveRequestLineItem
    {
        public long? LeaveRequestLineItemId { get; set; }
        public long LeaveRequestId { get; set; }
        public long RelieverUserId { get; set; }
        public int LeaveLength { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public DateTime? ResumptionDate { get; set; }
        public string RescheduleReason { get; set; } = string.Empty;
        public bool IsRescheduled { get; set; } = false;
        public string HandoverNotes { get; set; } = string.Empty;
        public long EmployeeId { get; set; }
        public long LeaveTypeId { get; set; }
        public long CompanyId { get; set; }
    }

}
