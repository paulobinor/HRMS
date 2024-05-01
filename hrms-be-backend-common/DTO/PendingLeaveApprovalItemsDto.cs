namespace hrms_be_backend_common.DTO
{
    public class PendingLeaveApprovalItemsDto
    {

        public long LeaveApprovalLineItemId { get; set; }
        public long LeaveApprovalId { get; set; }
        public bool IsApproved { get; set; }
        public long ApprovalEmployeeId { get; set; }
        public string Comments { get; set; }
        public string ApprovalPosition { get; set; }
        public string FullName { get; set; }
        public string LeaveTypeName { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ResumptionDate { get; set; }
        public int LeaveLength { get; set; }
        public int ApprovalStep { get; set; }
        public int CurrentApprovalID { get; set; }
        
    }
}
