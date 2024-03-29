using System.Numerics;

namespace hrms_be_backend_data.RepoPayload
{

    public class LeaveRequest
    {
        public long EmployeeId { get; set; }
        public long LeaveRequestID { get; set; }
        public long CompanyID { get; set; } = 0;
        public int CurrentlSplitCount { get; set; }
        public int MaxSplitCount { get; set; }
        public int TotalDays { get; set; }
        public string LeavePeriod { get; set; }


        public byte IsMDDeclined { get; set; } = 0;
        public byte IsMDApproved { get; set; } = 0;
        public byte IsUnitHeadDeclined { get; set; } = (byte)0;
        public byte? IsHrDeclined { get; set; } = 0;


        public bool IsUnitHeadApproved { get; set; } = false;
        public DateTime? UnitHeadDateApproved { get; set; }
        public DateTime? UnitHeadDateDisapproved { get; set; }

    }
    public class LeaveRequestDTO
    {
        public long LeaveRequestID { get; set; }
        public long UserId { get; set; } = 0;
        public string LeavePeriod { get; set; }
        public long LeaveTypeId { get; set; }
        public long NoOfDays { get; set; } = 0;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } 
        public long ReliverUserID { get; set; }
        public string LeaveEvidence { get; set; }
        public string Notes { get; set; }
        public long UnitHeadUserID { get; set; } = 0;
        public long HodUserID { get; set; } = 0;
        public long HRUserId { get; set; } = 0;
 
        public  string ReasonForRescheduling { get; set; }
        public long CompanyID { get; set; }
        public string StaffName { get; set; }
        public string ReliverName { get; set; }
        public string LeaveTypeName { get; set; }
       

        public DateTime DateCreated { get; set; }
        public string Created_By_User_Email { get; set; }

        public bool IsUpdated { get; set; } = false;
        public DateTime? Updated_Date { get; set; }
        public string Updated_By_User_Email { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? Deleted_Date { get; set; }
        public string Deleted_By_User_Email { get; set; }
        public string Reasons_For_Delete { get; set; }

        public bool IsUnitHeadApproved { get; set; } = false;
        public DateTime? UnitHeadDateApproved { get; set; }
        public bool IsUnitHeadDeclined { get; set; } = false;
        public DateTime? UnitHeadDateDisapproved { get; set; }
        public string UnitHeadDisapprovedComment { get; set; }

        public bool IsHodApproved { get; set; } = false;
        public DateTime? HodDateApproved { get; set; }
        public bool IsHodDeclined { get; set; } = false;
        public string HodDisapprovedComment { get; set; }
        public DateTime? HodDateDisapproved { get; set; }
        public bool IsHrApproved { get; set; } = false;
        public DateTime? HrDateApproved { get; set; }
        public bool IsHrDeclined { get; set; } = false;
        public string HrDisapprovedComment { get; set; }
        public DateTime? HrDateDisapproved { get; set; }
        public long EmployeeId { get; set; }
    }

    public class LeaveRequestCreate
    {
        public long LeaveRequestId { get; set; }
        public long EmployeeId { get; set; }
        public long GradeLevelId { get; set; }
        public int TotalDays { get; set; }
        public int MaximumSplitCount { get; set; }
        public int CurrentSplitCount { get; set; }
        public int RemainingDays { get; set; }
       // public DateTime LeavePeriod { get; set; }
        public int RequestYear { get; set; }
        public long LeaveTypeId { get; set; }
        public long NoOfDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long RelieverUserID { get; set; }
        public string LeaveEvidence { get; set; }
        public string Notes { get; set; }
   
        public string ReasonForRescheduling { get; set; }
        public long CompanyID { get; set; }

        public string Created_By_User_Email { get; set; }
    }

    public class RescheduleLeaveRequest
    {
        public long LeaveRequestID { get; set; }
        public long EmployeeId { get; set; }
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
