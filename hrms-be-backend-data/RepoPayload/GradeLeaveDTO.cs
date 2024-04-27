namespace hrms_be_backend_data.RepoPayload
{
    public  class GradeLeaveDTO
    {
        public long GradeLeaveID { get; set; }
        public long LeaveTypeId { get; set; }
        public long GradeID { get; set; }
        public long NumbersOfDays { get; set; }
        public long NumberOfVacationSplit { get; set; }
        public int MaximumNumberOfLeaveDays { get; set; }
        
        public string GradeName { get; set; }
        public string LeaveTypeName { get; set; }
        public long CompanyID { get; set; }

      //  public DateTime Created_Date { get; set; }
      //  public string Created_By_User_Email { get; set; }

        public bool IsUpdated { get; set; }
        public DateTime? Updated_Date { get; set; }
       // public string Updated_By_User_Email { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DateDeleted { get; set; }
        public string UserId { get; set; }
        public string DeletedComment { get; set; }
    }

    public class CreateGradeLeaveDTO
    {

        public long LeaveTypeId { get; set; }
        public long GradeID { get; set; }
        public long NumbersOfDays { get; set; }
        public long NumberOfVacationSplit { get; set; }
        public string GradeName { get; set; }
        public string LeaveTypeName { get; set; }
        public long CompanyID { get; set; }

        public DateTime Created_Date { get; set; }
        public long CreatedByUserID { get; set; }

        //public bool IsUpdated { get; set; }
        //public DateTime? Updated_Date { get; set; }
        //public string Updated_By_User_Email { get; set; }

        //public bool IsDeleted { get; set; }
        //public DateTime? Deleted_Date { get; set; }
        //public string Deleted_By_User_Email { get; set; }
        //public string Reasons_For_Delete { get; set; }
    }

    public class UpdateGradeLeaveDTO
    {
        public long GradeLeaveID { get; set; }
        public long LeaveTypeId { get; set; }
        public long GradeID { get; set; }
        public long NumbersOfDays { get; set; }
        public long NumberOfVacationSplit { get; set; }
        public long CompanyID { get; set; }
        public long UserId { get; set; }
        public int MaximumNumberOfLeaveDays { get; set; }
    }

    public class DeleteGradeLeaveDTO
    {
        public long GradeLeaveID { get; set; }
        public string Reasons_For_Delete { get; set; }
        public long UserID { get; set; }
    }
}
