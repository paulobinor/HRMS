using System;

namespace Com.XpressPayments.Data.LeaveModuleDTO.DTO
{
    public class RescheduleLeaveRequestDTO
    {
        public long RescheduleLeaveRequestID { get; set; }
        public long LeaveRequestID { get; set; }
        public long UserId { get; set; }
        public string RequestYear { get; set; }
        public string LeaveTypeId { get; set; }
        public long NoOfDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long ReliverUserID { get; set; }
        public string LeaveEvidence { get; set; }
        public string Notes { get; set; }
        public long UnitHeadUserID { get; set; }
        public long HodUserID { get; set; }
        public long HRUserId { get; set; }
        public string ReasonForRescheduling { get; set; }
        public long CompanyID { get; set; }

        public DateTime Created_Date { get; set; }
        public string Created_By_User_Email { get; set; }

        public bool IsUpdated { get; set; }
        public DateTime? Updated_Date { get; set; }
        public string Updated_By_User_Email { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? Deleted_Date { get; set; }
        public string Deleted_By_User_Email { get; set; }
        public string Reasons_For_Delete { get; set; }

        public bool IsHodApproved { get; set; }
    }

    public class RescheduleLeaveRequestCreate
    {
        public long RescheduleLeaveRequestID { get; set; }       
        public int RequestYear { get; set; }
        public string LeaveTypeId { get; set; }
        public long NoOfDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long ReliverUserID { get; set; }
        public string LeaveEvidence { get; set; }
        public string Notes { get; set; }
        public string ReasonForRescheduling { get; set; }       
    }

    public class RescheduleLeaveRequestDelete
    {
        public long RescheduleLeaveRequestID { get; set; }
        public string Reasons_For_Delete { get; set; }
    }
    public class RescheduleLeaveRequestDisapproved
    {
        public long RescheduleLeaveRequestID { get; set; }
        public string Reasons_For_Disapprove { get; set; }
    }
}
