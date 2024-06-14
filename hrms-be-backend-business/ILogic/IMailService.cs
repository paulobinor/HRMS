using hrms_be_backend_common.DTO;

namespace hrms_be_backend_business.ILogic
{
    public interface IMailService
    {
        Task SendEmailApproveUser(string recipientEmail, string recipientName, string defaultPass, string subject, string token);
        Task SendEmailAsync(MailRequest mailRequest, string attarchDocument);
        void SendEmail(string recipientEmail, string firtname, string defaultPass, string subject, string wwwRootPath, string ip, string port, string appKey = null, string channel = null);
        Task SendLeaveMailToReliever(long RelieverUserId, long leaveRequetedByUserId, DateTime startDate, DateTime endDate);
        Task SendLeaveApproveMailToApprover(long ApprovalEmployeeId, long leaveRequetedByEmployeeId, DateTime startDate, DateTime endDate);
        Task SendLeaveApproveConfirmationMail(long RequesterEmployeeId, long ApprovedByEmployeeId, DateTime startDate, DateTime endDate);
        Task SendLeaveDisapproveConfirmationMail(long RequesterEmployeeId, long DiapprovedByEmployeeId);


        Task SendResignationMailFromHrToStaff(long ResigationByEmployeeId, DateTime lastDatOfWork);
        Task SendResignationApproveMailToApprover(long ApproverEmployeeId, long ResigationByEmployeeId, DateTime lastDatOfWork);
        Task SendResignationApproveConfirmationMail(long RequesterEmployeeId, long ApprovedByEmployeeId, DateTime lastDatOfWork);
        Task SendResignationDisapproveConfirmationMail(long RequesterEmployeeId, long DiapprovedByEmployeeId);     
        Task SendResignationClearanceApproveMailToApprover(long ApproverEmployeeId, long ResigationByEmployeeId);
        Task SendResignationClearanceApproveConfirmationMail(long RequesterEmployeeId, long ApprovedByEmployeeId);
        Task SendResignationClearanceDisapproveConfirmationMail(long RequesterEmployeeId, long DiapprovedByEmployeeId, string reason);

    }
}
