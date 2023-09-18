namespace hrms_be_backend_business.ILogic
{
    public interface IMailService
    {
        Task SendLeaveMailToReliever(long RelieverUserId, long leaveRequetedByUserId, DateTime startDate, DateTime endDate);
        Task SendLeaveApproveMailToApprover(long ApprovalUserId, long leaveRequetedByUserId, DateTime startDate, DateTime endDate);
        Task SendLeaveApproveConfirmationMail(long RequesterUserId, long ApprovedByUserId, DateTime startDate, DateTime endDate);
        Task SendLeaveDisapproveConfirmationMail(long RequesterUserId, long DiapprovedByUserId);
    }
}
