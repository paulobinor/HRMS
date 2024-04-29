using hrms_be_backend_common.Models;

namespace hrms_be_backend_common.DTO
{
    public class LeaveApprovalInfoDto
    {
        public long LeaveApprovalId { get; set; }
        public long LeaveRequestLineItemId { get; set; }
        public long EmployeeId { get; set; }
        public long CompanyID { get; set; }
        public string? Comments { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public string ApprovalStatus { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? LeaveLength { get; set; }
        public DateTime ResumptionDate { get; set; }
        public bool IsRescheduled { get; set; }
        public string? HandOverNotes { get; set; }
        public int? RescheduleCount { get; set; }
        public string? RescheduleReason { get; set; }
        public bool IsApproved { get; set; }
        public string? FullName { get; set; }
        public string? RelieverName { get; set; }
        
        public int RequiredApprovalCount { get; set; }
        public int CurrentApprovalCount { get; set; }
        public List<LeaveApprovalLineItem>? LeaveApprovalLineItems { get; set; }
    }

}
