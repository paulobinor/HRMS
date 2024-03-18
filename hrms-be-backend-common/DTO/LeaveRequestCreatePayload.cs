using System;

namespace hrms_be_backend_common.DTO
{
    public class LeaveRequestCreatePayload
    {
        public string StaffID { get; set; }
        public string RequestYear { get; set; }
        public string LeaveTypeId { get; set; }
        public long NoOfDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ReliverStaffID { get; set; }
        public string LeaveEvidence { get; set; }
        public string Notes { get; set; }
        public string ReasonForRescheduling { get; set; }       
    }
}
