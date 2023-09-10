namespace hrms_be_backend_data.RepoPayload
{
    public class LeaveRequestDTO
    {
        public long LeaveRequestID { get; set; }
        public long UserId { get; set; }
        public  string RequestYear { get; set; }
        public long LeaveTypeId { get; set; }
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
        public string StaffName { get; set; }
        public string ReliverName { get; set; }
        public string LeaveTypeName { get; set; }
       

        public DateTime DateCreated { get; set; }
        public string Created_By_User_Email { get; set; }

        public bool IsUpdated { get; set; }
        public DateTime? Updated_Date { get; set; }
        public string Updated_By_User_Email { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? Deleted_Date { get; set; }
        public string Deleted_By_User_Email { get; set; }
        public string Reasons_For_Delete { get; set; }
        public bool IsUnitHeadApproved { get; set; }
        public DateTime? UnitHeadDateApproved { get; set; }
        public bool IsUnitHeadDeclined { get; set; }
        public DateTime? UnitHeadDateDisapproved { get; set; }
        public string UnitHeadDisapprovedComment { get; set; }
        public bool IsHodApproved { get; set; }
        public DateTime? HodDateApproved { get; set; }
        public bool IsHodDeclined { get; set; }
        public string HodDisapprovedComment { get; set; }
        public DateTime? HodDateDisapproved { get; set; }
        public bool IsHrApproved { get; set; }
        public DateTime? HrDateApproved { get; set; }
        public bool IsHrDeclined { get; set; }
        public string HrDisapprovedComment { get; set; }
        public DateTime? HrDateDisapproved { get; set; }


    }

    public class LeaveRequestCreate
    {

        public long UserId { get; set; }
        public int RequestYear { get; set; }
        public long LeaveTypeId { get; set; }
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

    public class RescheduleLeaveRequest
    {
        public long LeaveRequestID { get; set; }
        public long UserId { get; set; }
        public long RequestYear { get; set; }
        public long LeaveTypeId { get; set; }
        public long NoOfDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long ReliverUserID { get; set; }
        public string LeaveEvidence { get; set; }
        public string Notes { get; set; }
        public string ReasonForRescheduling { get; set; }
        public long CompanyID { get; set; }

       
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
