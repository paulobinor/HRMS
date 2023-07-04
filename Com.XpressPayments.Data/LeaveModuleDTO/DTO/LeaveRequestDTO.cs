using System;

namespace Com.XpressPayments.Data.LeaveModuleDTO.DTO
{
    public class LeaveRequestDTO
    {
        public long LeaveRequestID { get; set; }
        public long UserId { get; set; }
        public  string RequestYear { get; set; }
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
        public  string ReasonForRescheduling { get; set; }
        public long CompanyID { get; set; }
        public bool IsHodApproved { get; set; }

        public DateTime Created_Date { get; set; }
        public string Created_By_User_Email { get; set; }

        public bool IsUpdated { get; set; }
        public DateTime? Updated_Date { get; set; }
        public string Updated_By_User_Email { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? Deleted_Date { get; set; }
        public string Deleted_By_User_Email { get; set; }
        public string Reasons_For_Delete { get; set; }

        
    }

    public class LeaveRequestCreate
    {

        public long UserId { get; set; }
        public int RequestYear { get; set; }
        public string LeaveTypeId { get; set; }
        public long NoOfDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long ReliverUserID { get; set; }
        public string LeaveEvidence { get; set; }
        public string Notes { get; set; }     
        public string ReasonForRescheduling { get; set; }
        public long CompanyID { get; set; }

        public string Created_By_User_Email { get; set; }
    }

    public class LeaveRequestDelete
    {
        public long LeaveRequestID { get; set; }
        public string Reasons_For_Delete { get; set; }
    }
    public class LeaveRequestDisapproved
    {
        public long LeaveRequestID { get; set; }
        public string Reasons_For_Disapprove { get; set; }
    }

}
