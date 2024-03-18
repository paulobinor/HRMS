namespace hrms_be_backend_data.RepoPayload
{
    public class SupervisorTrainingFeedbackFormDTO
    {
        public long SupervisorTrainingFeedbackFormID { get; set; }
        public long EmployeeId { get; set; }
        public long CompanyID { get; set; }
        public string CourseTitle { get; set; }
        public string EmployeeName { get; set; }
        public long DepartmentID { get; set; }
        public DateTime Date { get; set; }
        public string SupervisorName { get; set; }
        public string Facilitator { get; set; }
        public string ReviewPeriod { get; set; }
        public string ThreeImpactsofTraningOnEmployee { get; set; }
        public int TrainingDifferenceOnEmployeeKnowledge { get; set; }
        public int TrainingDifferenceOnEmployeeEfficiency { get; set; }
        public int TrainingDifferenceOnEmployeeTAT { get; set; }
        public int TrainingDifferenceOnEmployeeCareerGrowth { get; set; }
        public string WhatAdditionalTrainingDevelopmentEducationDoYouRequire { get; set; }
        public string Comments { get; set; }
        public DateTime Created_Date { get; set; }
        public string Created_By_User_Email { get; set; }

    }
    public class SupervisorTrainingFeedbackFormCreate
    {
        public long EmployeeId { get; set; }
        public long CompanyID { get; set; }
        public string CourseTitle { get; set; }
        public string EmployeeName { get; set; }
        public long DepartmentID { get; set; }
        public DateTime Date { get; set; }
        public string SupervisorName { get; set; }
        public string Facilitator { get; set; }
        public string ReviewPeriod { get; set; }
        public string ThreeImpactsofTraningOnEmployee { get; set; }
        public int TrainingDifferenceOnEmployeeKnowledge { get; set; }
        public int TrainingDifferenceOnEmployeeEfficiency { get; set; }
        public int TrainingDifferenceOnEmployeeTAT { get; set; }
        public int TrainingDifferenceOnEmployeeCareerGrowth { get; set; }
        public string WhatAdditionalTrainingDevelopmentEducationDoYouRequire { get; set; }
        public string Comments { get; set; }
        public DateTime Created_Date { get; set; }
        public string Created_By_User_Email { get; set; }

    }
}
