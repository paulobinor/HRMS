using hrms_be_backend_common.DTO;

namespace hrms_be_backend_business.ILogic
{
    public interface IMailService
    {
        void SendEmailApproveUser(string recipientEmail, string recipientName, string defaultPass, string subject, string token);
        Task SendEmailAsync(MailRequest mailRequest, string attarchDocument);
        void SendEmail(string recipientEmail, string firtname, string defaultPass, string subject, string wwwRootPath, string ip, string port, string appKey = null, string channel = null);
        Task SendLeaveMailToReliever(long RelieverUserId, long leaveRequetedByUserId, DateTime startDate, DateTime endDate);
        Task SendLeaveApproveMailToApprover(long ApprovalEmployeeId, long leaveRequetedByEmployeeId, DateTime startDate, DateTime endDate);
        Task SendLeaveApproveConfirmationMail(long RequesterEmployeeId, long ApprovedByEmployeeId, DateTime startDate, DateTime endDate);
        Task SendLeaveDisapproveConfirmationMail(long RequesterEmployeeId, long DiapprovedByEmployeeId);
    }
}
