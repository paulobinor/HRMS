namespace hrms_be_backend_api.Dtos
{
    public class UpdateLeaveRequestLineItemDto
    {
        //  public long? LeaveRequestLineItemId { get; set; }
        //  public long LeaveRequestId { get; set; }
        public long RelieverUserId { get; set; }
        public int LeaveLength { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public DateTime? ResumptionDate { get; set; }
        public string RescheduleReason { get; set; } = string.Empty;
        // public bool IsRescheduled { get; set; } = false;
        public string HandoverNotes { get; set; } = string.Empty;
        public long EmployeeId { get; set; }
        public long LeaveTypeId { get; set; }
        public long CompanyId { get; set; }
        //  public bool IsApproved { get; set; }
    }


}
