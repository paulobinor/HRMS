namespace hrms_be_backend_common.Models
{
    public class LeaveRequestLineItem
    {
        public long? LeaveRequestLineItemId { get; set; }
        public long? leaveApprovalLineItemId { get; set; }
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
        public bool IsApproved { get; set; } = false;
        public string? UploadFilePath { get; set; } = string.Empty;
        public int? AnnualLeaveId { get; set; }
        public int? ApproalID { get; set; }
        public long? leaveApprovalId { get; set; }
        public string? Comments { get; set; }
        public string? ApprovalStatus { get; set; }
        public string? RelieverName { get; set; }
        public string? LeaveTypeName { get; set; }
    }

    public class CreateLeaveRequestLineItem
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
        public string? UploadFilePath { get; set; } = string.Empty;
        public long EmployeeId { get; set; }
        public long LeaveTypeId { get; set; }
        public long CompanyId { get; set; }
      //  public bool IsApproved { get; set; }
    }

    public class CreateAnnualLeavePlan
    {
        public List<CreateLeaveRequestLineItem> LeaveRequestLineItems { get; set; }
        public int LeaveLength { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public DateTime? ResumptionDate { get; set; }
        public string RescheduleReason { get; set; } = string.Empty;
        // public bool IsRescheduled { get; set; } = false;
        public string HandoverNotes { get; set; } = string.Empty;
        public string? UploadFilePath { get; set; } = string.Empty;
        public long EmployeeId { get; set; }
        public long LeaveTypeId { get; set; }
        public long CompanyId { get; set; }
        //  public bool IsApproved { get; set; }
    }


}
