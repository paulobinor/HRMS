
namespace hrms_be_backend_common.DTO
{
    public class LeaveRequestLineItemDto
    {

        public long? LeaveRequestLineItemId { get; set; }
        public long LeaveRequestId { get; set; }
        public long LeaveApprovalLineItemId { get; set; }
        public long LeaveApprovalId { get; set; }
        public long RelieverUserId { get; set; }
        public int LeaveLength { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public DateTime? ResumptionDate { get; set; }
        public string RescheduleReason { get; set; } = string.Empty;
        public bool IsRescheduled { get; set; } = false;
        public string HandoverNotes { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string LeaveTypeName { get; set; } = string.Empty;
        public string ApprovalStatus { get; set; }
        public long EmployeeId { get; set; }
        public long GradeID { get; set; }
        public long LeaveTypeId { get; set; }
        public long CompanyId { get; set; }
        public bool IsApproved { get; set; }
        public string Comments { get; set; }
        public string RelieverName { get; set; }
        public string UploadFilePath { get; set; }
        
        public List<LeaveApprovalLineItemDto> leaveApprovalLineItems { get; set; } // = new List<LeaveApprovalLineItemDto>();
        public int AnnualLeaveId { get; set; }
    }


}
