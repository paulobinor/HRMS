namespace hrms_be_backend_business.ILogic
{
    public interface ILearningAndDevelopmentMailService
    {
        //Task SendTrainingPlanToHod(long HodUserId, long TrainingPlanByUserId, string TrainingProvider);
        Task SendTrainingPlanApprovalMailToApprover(long ApproverUserId, long TrainingPlanByUserId, string TrainingProvider);
        Task SendTrainingPlanApproveConfirmationMail(long RequesterUserId, long ApprovedByUserId, string TrainingProvider);
        Task SendTrainingPlanDisapproveConfirmationMail(long RequesterUserId, long DiapprovedByUserId);
        Task SendTrainingScheduleNotificationMail(long StaffUserId, string TrainingOrganizer, string TrainingVenue, string TrainingMode, string TrainingTime, string TrainingTopic, string TrainingDate);
    }
}
