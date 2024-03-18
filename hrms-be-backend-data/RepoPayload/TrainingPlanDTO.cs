namespace hrms_be_backend_data.RepoPayload
{
    public class TrainingPlanDTO
    {
        public long TrainingPlanID { get; set; }
        public long EmployeeID { get; set; }
        public long CompanyID { get; set; }
        public string StaffName { get; set; }
        public string Department { get; set; }
        public DateTime Created_Date { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDisapproved { get; set; }
        public string ApprovedBy { get; set; }
        public string TrainingProvider { get; set; }
        public string TrainingNeeds { get; set; }
        public string IdentifiedSkills { get; set; }
        public decimal EstimatedCost { get; set; }
        public string TrainingOrganizer { get; set; }
        public string TrainingVenue { get; set; }
        public string TrainingTopic { get; set; }
        public string TrainingTime { get; set; }
        public string TrainingMode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RequestedBy { get; set; }


        public long UnitHeadEmployeeID { get; set; }
        public bool IsUnitHeadApproved { get; set; }
        public DateTime UnitHeadDateApproved { get; set; }
        public bool IsUnitHeadDisapproved { get; set; }
        public DateTime UnitHeadDateDisapproved { get; set; }
        public string UnitHeadDisapprovedComment { get; set; }


        public long HodEmployeeID { get; set; }
        public bool IsHodApproved { get; set; }
        public DateTime HodDateApproved { get; set; }
        public bool IsHodDisapproved { get; set; }
        public string HodDisapprovedComment { get; set; }
        public DateTime HodDateDisapproved { get; set; }
        public long HREmployeeId { get; set; }
        
      
        public long HrEmployeeID { get; set; }
        public bool IsHrApproved { get; set; }
        public DateTime HrDateApproved { get; set; }
        public bool IsHrDisapproved { get; set; }
        public string HrDisapprovedComment { get; set; }
        public DateTime HrDateDisapproved { get; set; }
        public bool IsUpdated { get; set; }
        public DateTime? Updated_Date { get; set; }
        public string Updated_By_User_Email { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? Deleted_Date { get; set; }
        public string Deleted_By_User_Email { get; set; }
        public string Reasons_For_Delete { get; set; }
        public bool IsScheduled { get; set; }
        public DateTime? Scheduled_Date { get; set; }
        public string Scheduled_By_User_Email { get; set; }


    }
    public class TrainingPlanCreate
    {
        public long EmployeeID { get; set; }
        public string StaffName { get; set; }
        public string Department { get; set; }
        public string IdentifiedSkills { get; set; }
        public string TrainingNeeds { get; set; }

        public string TrainingProvider { get; set; }
        public decimal EstimatedCost { get; set; }
        public long CompanyID { get; set; }
        public string RequestedBy { get; set; }
        public bool IsHodApproved { get; set; }
        public bool IsUnitHeadApproved { get; set; }
        public bool IsHrApproved { get; set; }
        public DateTime Created_Date { get; set; }
        public string Created_By_User_Email { get; set; }
        // public bool IsDeleted { get; set; }

    }
    public class TrainingPlanUpdate
    {
        public long TrainingPlanID { get; set; }
        public string IdentifiedSkills { get; set; }
        public string TrainingNeeds { get; set; }

        public string TrainingProvider { get; set; }
        public decimal EstimatedCost { get; set; }
        public long CompanyID { get; set; }
        //public string Updated_By_User_Email { get; set; }
    }

    public class TrainingPlanSchedule
    {
        public long TrainingPlanID { get; set; }
        public string TrainingOrganizer { get; set; }
        public string TrainingVenue { get; set; }
        public string TrainingTopic { get; set; }
        public string TrainingTime { get; set; }
        public string TrainingMode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long CompanyID { get; set; }
    }
    public class TrainingPlanApproved
    {
        public long TrainingPlanID { get; set; }
    }
    public class TrainingPlanDisapproved
    {
        public long TrainingPlanID { get; set; }
        public string Reasons_For_Disapprove { get; set; }
    }
    public class TrainingPlanDelete
    {
        public long TrainingPlanID { get; set; }
        public string Reasons_For_Delete { get; set; }
    }
}
