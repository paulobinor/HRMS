namespace hrms_be_backend_data.RepoPayload
{
    public class TraineeTrainingFeedbackFormDTO
    {
        public long UserId { get; set; }
        public long TraineeTrainingFeedbackFormID { get; set; }
        public long CompanyID { get; set; }
        public string CourseTitle { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public DateTime Date { get; set; }
        public string Venue { get; set; }
        public string TrainerName { get; set; }
        public string ThreeThingsLearned { get; set; }
        public string TrainingDifferenceOnEmployeeJob { get; set; }
        public string WasAppropraiteMaterialCovered { get; set; }
        public string RateTraining_Expertise { get; set; }
        public string RateTraining_Clarity { get; set; }
        public string RateTraining_CulturallyAppropriate { get; set; }
        public string RateTraining_Responsiveness { get; set; }
        public string WhatAdditionalTrainingDevelopmentEducationDoYouRequire { get; set; }
        public string Comments { get; set; }
        public string Created_By_User_Email { get; set; }
        public DateTime Created_Date { get; set; }
    }
    public class TraineeTrainingFeedbackFormCreate
    {
        public long UserId { get; set; }
        public long CompanyID { get; set; }
        public string CourseTitle { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public DateTime Date { get; set; }
        public string Venue { get; set; }
        public string TrainerName { get; set; }
        public string ThreeThingsLearned { get; set; }
        public string TrainingDifferenceOnEmployeeJob { get; set; }
        public string WasAppropraiteMaterialCovered { get; set; }
        public string RateTraining_Expertise { get; set; }
        public string RateTraining_Clarity { get; set; }
        public string RateTraining_CulturallyAppropriate { get; set; }
        public string RateTraining_Responsiveness { get; set; }
        public string WhatAdditionalTrainingDevelopmentEducationDoYouRequire { get; set; }
        public string Comments { get; set; }
        public string Created_By_User_Email { get; set; }
        public DateTime Created_Date { get; set; }
    }
}
