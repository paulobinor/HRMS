﻿namespace hrms_be_backend_data.RepoPayload
{
    public class LeaveTypeDTO
    {
        public long LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
       // public long MaximumLeaveDurationDays { get; set; }
      //  public string Gender { get; set; }
      //  public bool IsPaidLeave { get; set; } 
        public long CompanyID { get; set; }

       // public DateTime Created_Date { get; set; }
       // public string Created_By_User_Email { get; set; }

       // public bool IsUpdated { get; set; }
       // public DateTime? Updated_Date { get; set; }
       // public string Updated_By_User_Email { get; set; }

       // public bool IsDeleted { get; set; }
      //  public DateTime? Deleted_Date { get; set; }
      //  public string Deleted_By_User_Email { get; set; }
       // public string Reasons_For_Delete { get; set; }
    }

    public class CreateLeaveTypeDTO
    {

        public string LeaveTypeName { get; set; }
      //  public long MaximumLeaveDurationDays { get; set; }
      //  public string Gender { get; set; }
        //public bool IsPaidLeave { get; set; } 
        public long CompanyID { get; set; }
        public string UserId { get; set; }
        public long GenderID { get; internal set; }
    }

    public class UpdateLeaveTypeDTO
    {

        public long LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
        public long LastUpdatedUserId { get; set; }
        public long CompanyID { get; set; }
        public long GenderID { get; internal set; }
    }

    public class DeleteLeaveTypeDTO
    {
        public long LeaveTypeId { get; set; }
        public string Reasons_For_Delete { get; set; }
        public long DeletedByUserId { get; set; }
    }
}
