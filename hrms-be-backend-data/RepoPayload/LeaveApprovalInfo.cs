using hrms_be_backend_common.Models;

namespace hrms_be_backend_data.RepoPayload
{
    public class LeaveApprovalInfo
    {
        public long LeaveApprovalId { get; set; }
        public long LeaveRequestLineItemId { get; set; }
        public int RequiredApprovalCount { get; set; }
        public int CurrentApprovalCount { get; set; }
        public int CurrentApprovalID { get; set; }
        public string? Comments { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public string ApprovalStatus { get; set; }
        public bool IsApproved { get; set; } = false;
        public List<LeaveApprovalLineItem>? LeaveApprovalLineItems { get; set; }
    }

}
