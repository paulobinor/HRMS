using Com.XpressPayments.Common.Configuration;
using Com.XpressPayments.Common.DTO.Mail;
using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
using Com.XpressPayments.Data.Repositories.UserAccount.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Util
{
    public class MailService: IMailService
    {
        private readonly Smtp _smtpParameters;
        private readonly ILogger<MailService> _logger;
        private readonly IAccountRepository _accountRepository;
        public MailService(IOptions<Smtp> smtpParameters, ILogger<MailService> logger, IAccountRepository accountRepository)
        {
            _smtpParameters = smtpParameters.Value;
            _accountRepository = accountRepository;
            _logger = logger;
           
        }
        public async Task SendEmailAsync(MailRequest mailRequest, string attarchDocument)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(_smtpParameters.EmailAddress, _smtpParameters.DisplayName);
                mail.To.Add(mailRequest.ToEmail);
                mail.Subject = mailRequest.Subject;
                mail.Body = mailRequest.Body;
                Attachment attachment;
                if (attarchDocument != null)
                {
                    attachment = new Attachment(attarchDocument);
                    mail.Attachments.Add(attachment);
                }
                mail.IsBodyHtml = true;
                NetworkCredential Credentials = new NetworkCredential(_smtpParameters.EmailAddress, _smtpParameters.Password);
                using (var smtpClient = new SmtpClient(_smtpParameters.Host,Convert.ToInt32(_smtpParameters.Port)))
                {
                    smtpClient.Credentials = Credentials;
                    smtpClient.EnableSsl = _smtpParameters.SSL;
                    await smtpClient.SendMailAsync(mail);


                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), new { Controller = "MailService", Method = "SendEmailAsync" });
            }
        }
        public async Task SendLeaveMailToReliever(long RelieverUserId, long leaveRequetedByUserId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var userDetails = await _accountRepository.FindUser(RelieverUserId);
                var leaveRequetedBy = await _accountRepository.FindUser(leaveRequetedByUserId);
                StringBuilder mailBody = new StringBuilder();
                mailBody.Append($"Dear {userDetails.FirstName} {userDetails.LastName} {userDetails.MiddleName} <br/> <br/>");
                mailBody.Append($"You are to relieve {leaveRequetedBy.FirstName} {leaveRequetedBy.LastName} {leaveRequetedBy.MiddleName} <br/> <br/>");
                mailBody.Append($"<b>Start Date : <b/> {startDate}");
                mailBody.Append($"<b>End Date : <b/> {endDate}");

                var mailPayload = new MailRequest
                {
                    Body = mailBody.ToString(),
                    Subject = "Leave Request",
                    ToEmail = userDetails.Email,
                };
                SendEmailAsync(mailPayload, null);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), new { Controller = "MailService", Method = "SendLeaveMailToReliever" });
            }
        }
        public async Task SendLeaveApproveMailToApprover(long ApprovalUserId, long leaveRequetedByUserId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var userDetails = await _accountRepository.FindUser(ApprovalUserId);
                var leaveRequetedBy = await _accountRepository.FindUser(leaveRequetedByUserId);
                StringBuilder mailBody = new StringBuilder();
                mailBody.Append($"Dear {userDetails.FirstName} {userDetails.LastName} {userDetails.MiddleName} <br/> <br/>");
                mailBody.Append($"Kindly login to approve a leave request by {leaveRequetedBy.FirstName} {leaveRequetedBy.LastName} {leaveRequetedBy.MiddleName} <br/> <br/>");
                mailBody.Append($"<b>Start Date : <b/> {startDate}  <br/> ");
                mailBody.Append($"<b>End Date : <b/> {endDate}   <br/> ");

                var mailPayload = new MailRequest
                {
                    Body = mailBody.ToString(),
                    Subject = "Leave Request",
                    ToEmail = userDetails.Email,
                };
                SendEmailAsync(mailPayload, null);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), new { Controller = "MailService", Method = "SendLeaveMailToUnitHead" });
            }
        }       
        public async Task SendLeaveApproveConfirmationMail(long RequesterUserId, long ApprovedByUserId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var userDetails = await _accountRepository.FindUser(RequesterUserId);
                var ApprovedByUserDetails = await _accountRepository.FindUser(ApprovedByUserId);
                StringBuilder mailBody = new StringBuilder();
                mailBody.Append($"Dear {userDetails.FirstName} {userDetails.LastName} {userDetails.MiddleName} <br/> <br/>");
                mailBody.Append($"You leave has been approved by {ApprovedByUserDetails.FirstName} {ApprovedByUserDetails.LastName} {ApprovedByUserDetails.MiddleName} <br/> <br/>");
              
                mailBody.Append($"<b> Your leave start from : <b/> {startDate} <br/> ");
                mailBody.Append($"<b>and end on : <b/> {endDate} <br/> ");

                var mailPayload = new MailRequest
                {
                    Body = mailBody.ToString(),
                    Subject = "Leave Request",
                    ToEmail = userDetails.Email,
                };
                SendEmailAsync(mailPayload, null);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), new { Controller = "MailService", Method = "SendLeaveMailToUnitHead" });
            }
        }
        public async Task SendLeaveDisapproveConfirmationMail(long RequesterUserId, long DiapprovedByUserId)
        {
            try
            {
                var userDetails = await _accountRepository.FindUser(RequesterUserId);
                var disapprovedByUserDetails = await _accountRepository.FindUser(DiapprovedByUserId);

                StringBuilder mailBody = new StringBuilder();
                mailBody.Append($"Dear {userDetails.FirstName} {userDetails.LastName} {userDetails.MiddleName} <br/> <br/>");
                mailBody.Append($"You leave has been disapproved by {disapprovedByUserDetails.FirstName} {disapprovedByUserDetails.LastName} {disapprovedByUserDetails.MiddleName} <br/> <br/>");

               

                var mailPayload = new MailRequest
                {
                    Body = mailBody.ToString(),
                    Subject = "Leave Request",
                    ToEmail = userDetails.Email,
                };
                SendEmailAsync(mailPayload, null);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), new { Controller = "MailService", Method = "SendLeaveMailToUnitHead" });
            }
        }
    }
}
