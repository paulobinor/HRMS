namespace hrms_be_backend_data.RepoPayload
{
    public class TraineeTrainingFeedbackFormDTO
    {
        public long EmployeeId { get; set; }
        public long TraineeTrainingFeedbackFormID { get; set; }
        public long CompanyID { get; set; }
        public string CourseTitle { get; set; }
        public string EmployeeName { get; set; }
        public long DepartmentID { get; set; }
        public DateTime Date { get; set; }
        public string Venue { get; set; }
        public string TrainerName { get; set; }
        public string ThreeThingsLearned { get; set; }
        public int TrainingDifferenceOnEmployeeJob { get; set; }
        public string WasAppropraiteMaterialCovered { get; set; }
        public int RateTraining_Expertise { get; set; }
        public int RateTraining_Clarity { get; set; }
        public int RateTraining_CulturallyAppropriate { get; set; }
        public int RateTraining_Responsiveness { get; set; }
        public string WhatAdditionalTrainingDevelopmentEducationDoYouRequire { get; set; }
        public string Comments { get; set; }
        public string Created_By_User_Email { get; set; }
        public DateTime Created_Date { get; set; }
    }
    public class TraineeTrainingFeedbackFormCreate
    {
        public long EmployeeId { get; set; }
        public long CompanyID { get; set; }
        public string CourseTitle { get; set; }
        public string EmployeeName { get; set; }
        public long DepartmentID { get; set; }
        public DateTime Date { get; set; }
        public string Venue { get; set; }
        public string TrainerName { get; set; }
        public string ThreeThingsLearned { get; set; }
        public int TrainingDifferenceOnEmployeeJob { get; set; }
        public string WasAppropraiteMaterialCovered { get; set; }
        public int RateTraining_Expertise { get; set; }
        public int RateTraining_Clarity { get; set; }
        public int RateTraining_CulturallyAppropriate { get; set; }
        public int RateTraining_Responsiveness { get; set; }
        public string WhatAdditionalTrainingDevelopmentEducationDoYouRequire { get; set; }
        public string Comments { get; set; }
        public string Created_By_User_Email { get; set; }
        public DateTime Created_Date { get; set; }
    }
}
