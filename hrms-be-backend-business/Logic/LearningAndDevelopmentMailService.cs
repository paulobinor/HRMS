using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Configuration;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.IRepository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_business.Logic
{
    public class LearningAndDevelopmentMailService : ILearningAndDevelopmentMailService
    {
        private readonly SmtpConfig _smtpParameters;
        private readonly ILogger<LearningAndDevelopmentMailService> _logger;
        private readonly IAccountRepository _accountRepository;
        public LearningAndDevelopmentMailService(IOptions<SmtpConfig> smtpParameters, ILogger<LearningAndDevelopmentMailService> logger, IAccountRepository accountRepository)
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
                using (var smtpClient = new SmtpClient(_smtpParameters.Host, Convert.ToInt32(_smtpParameters.Port)))
                {
                    smtpClient.Credentials = Credentials;
                    smtpClient.EnableSsl = _smtpParameters.SSL;
                    await smtpClient.SendMailAsync(mail);


                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), new { Controller = "LearningAndDevelopmentMailService", Method = "SendEmailAsync" });
            }
        }

        //public async Task SendTrainingPlanToHod(long HodUserId, long TrainingPlanByUserId, string TrainingProvider)
        //{
        //    try
        //    {
        //        var userDetails = await _accountRepository.FindUser(HodUserId);
        //        var TrainingPlanBy = await _accountRepository.FindUser(TrainingPlanByUserId);
        //        StringBuilder mailBody = new StringBuilder();
        //        mailBody.Append($"Dear {userDetails.FirstName} {userDetails.LastName} {userDetails.MiddleName} <br/> <br/>");
        //        mailBody.Append($"You are to approve training plan request by {TrainingPlanBy.FirstName} {TrainingPlanBy.LastName} {TrainingPlanBy.MiddleName} <br/> <br/>");
        //        mailBody.Append($"<b>Training Provider : <b/> {TrainingProvider}");

        //        var mailPayload = new MailRequest
        //        {
        //            Body = mailBody.ToString(),
        //            Subject = "Training Plan",
        //            ToEmail = userDetails.Email,
        //        };
        //        SendEmailAsync(mailPayload, null);

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.ToString(), new { Controller = "MailService", Method = "SendTrainingPlanToHod" });
        //    }
        //}

        public async Task SendTrainingPlanApprovalMailToApprover(long ApproverUserId, long TrainingPlanByUserId, string TrainingProvider)
        {
            try
            {
                var userDetails = await _accountRepository.FindUser(ApproverUserId);
                var trainingPlanBy = await _accountRepository.FindUser(TrainingPlanByUserId);
                StringBuilder mailBody = new StringBuilder();
                mailBody.Append($"Dear {userDetails.FirstName} {userDetails.LastName} {userDetails.MiddleName} <br/> <br/>");
                mailBody.Append($"Kindly login to approve a training plan request by {trainingPlanBy.FirstName} {trainingPlanBy.LastName} {trainingPlanBy.MiddleName} <br/> <br/>");
                mailBody.Append($"<b>Training Proviedr : <b/> {TrainingProvider}  <br/> ");

                var mailPayload = new MailRequest
                {
                    Body = mailBody.ToString(),
                    Subject = "Training Plan",
                    ToEmail = userDetails.Email,
                };
                SendEmailAsync(mailPayload, null);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), new { Controller = "LearningAndDevelopmentMailService", Method = "SendTrainingPlanApprovalMailToApprover" });
            }
        }

        public async Task SendTrainingPlanApproveConfirmationMail(long RequesterUserId, long ApprovedByUserId, string TrainingProvider)
        {
            try
            {
                var userDetails = await _accountRepository.FindUser(RequesterUserId);
                var ApprovedByUserDetails = await _accountRepository.FindUser(ApprovedByUserId);
                StringBuilder mailBody = new StringBuilder();
                mailBody.Append($"Dear {userDetails.FirstName} {userDetails.LastName} {userDetails.MiddleName} <br/> <br/>");
                mailBody.Append($"Your training plan has been approved by {ApprovedByUserDetails.FirstName} {ApprovedByUserDetails.LastName} {ApprovedByUserDetails.MiddleName} <br/> <br/>");

                mailBody.Append($"<b> Training Provider : <b/> {TrainingProvider} <br/> ");

                var mailPayload = new MailRequest
                {
                    Body = mailBody.ToString(),
                    Subject = "Training Plan",
                    ToEmail = userDetails.Email,
                };
                SendEmailAsync(mailPayload, null);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), new { Controller = "LearningAndDevelopmentMailService", Method = "SendTrainingPlanApproveConfirmationMail" });
            }
        }

        public async Task SendTrainingPlanDisapproveConfirmationMail(long RequesterUserId, long DiapprovedByUserId)
        {
            try
            {
                var userDetails = await _accountRepository.FindUser(RequesterUserId);
                var disapprovedByUserDetails = await _accountRepository.FindUser(DiapprovedByUserId);

                StringBuilder mailBody = new StringBuilder();
                mailBody.Append($"Dear {userDetails.FirstName} {userDetails.LastName} {userDetails.MiddleName} <br/> <br/>");
                mailBody.Append($"Your training plan has been disapproved by {disapprovedByUserDetails.FirstName} {disapprovedByUserDetails.LastName} {disapprovedByUserDetails.MiddleName} <br/> <br/>");



                var mailPayload = new MailRequest
                {
                    Body = mailBody.ToString(),
                    Subject = "Training Plan",
                    ToEmail = userDetails.Email,
                };
                SendEmailAsync(mailPayload, null);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), new { Controller = "LearningAndDevelopmentMailService", Method = "SendTrainingPlanDisapproveConfirmationMail" });
            }
        }

        public async Task SendTrainingScheduleNotificationMail(long StaffUserId, string TrainingOrganizer, string TrainingVenue, string TrainingMode, string TrainingTime, string TrainingTopic, string TrainingDate)
        {
            try
            {
                var userDetails = await _accountRepository.FindUser(StaffUserId);


                //Mail to staff
                StringBuilder mailBody = new StringBuilder();
                mailBody.Append($"Dear {userDetails.FirstName} {userDetails.LastName} {userDetails.MiddleName} <br/> <br/>");
                mailBody.Append($"You have a training Scheduled <br/> <br/>");

                mailBody.Append($"<b> Training Organizer : <b/> {TrainingOrganizer} <br/> ");
                mailBody.Append($"<b> Training Topic : <b/> {TrainingTopic} <br/> ");
                mailBody.Append($"<b> Training Date : <b/> {TrainingDate} <br/> ");
                mailBody.Append($"<b> Training Time : <b/> {TrainingTime} <br/> ");
                mailBody.Append($"<b> Training Venue : <b/> {TrainingVenue} <br/> ");
                mailBody.Append($"<b> Training Mode : <b/> {TrainingMode} <br/> ");

                var mailPayload = new MailRequest
                {
                    Body = mailBody.ToString(),
                    Subject = "Training Schedule",
                    ToEmail = userDetails.Email,
                };
                SendEmailAsync(mailPayload, null);

                if (userDetails.UnitHeadUserId == null)
                {
                    //Mail to Hod
                    var HodDetails = await _accountRepository.FindUser(userDetails.HODUserId);
                    StringBuilder HodmailBody = new StringBuilder();
                    mailBody.Append($"Dear {HodDetails.FirstName} {HodDetails.LastName} {HodDetails.MiddleName} <br/> <br/>");
                    mailBody.Append($"{userDetails.FirstName} {userDetails.LastName} {userDetails.MiddleName} have been Scheduled for training <br/> <br/>");

                    mailBody.Append($"<b> Training Organizer : <b/> {TrainingOrganizer} <br/> ");
                    mailBody.Append($"<b> Training Topic : <b/> {TrainingTopic} <br/> ");
                    mailBody.Append($"<b> Training Date : <b/> {TrainingDate} <br/> ");
                    mailBody.Append($"<b> Training Time : <b/> {TrainingTime} <br/> ");
                    mailBody.Append($"<b> Training Venue : <b/> {TrainingVenue} <br/> ");
                    mailBody.Append($"<b> Training Mode : <b/> {TrainingMode} <br/> ");

                    var HodmailPayload = new MailRequest
                    {
                        Body = mailBody.ToString(),
                        Subject = "Training Schedule",
                        ToEmail = HodDetails.Email,
                    };
                    SendEmailAsync(HodmailPayload, null);

                }
                else
                {
                    //Mail to UnitHead
                    var unitHeadDetails = await _accountRepository.FindUser(userDetails.UnitHeadUserId);
                    StringBuilder UnitHeadmailBody = new StringBuilder();
                    mailBody.Append($"Dear {unitHeadDetails.FirstName} {unitHeadDetails.LastName} {unitHeadDetails.MiddleName} <br/> <br/>");
                    mailBody.Append($"{userDetails.FirstName} {userDetails.LastName} {userDetails.MiddleName} have been Scheduled for training <br/> <br/>");

                    mailBody.Append($"<b> Training Organizer : <b/> {TrainingOrganizer} <br/> ");
                    mailBody.Append($"<b> Training Topic : <b/> {TrainingTopic} <br/> ");
                    mailBody.Append($"<b> Training Date : <b/> {TrainingDate} <br/> ");
                    mailBody.Append($"<b> Training Time : <b/> {TrainingTime} <br/> ");
                    mailBody.Append($"<b> Training Venue : <b/> {TrainingVenue} <br/> ");
                    mailBody.Append($"<b> Training Mode : <b/> {TrainingMode} <br/> ");

                    var UnitHeadmailPayload = new MailRequest
                    {
                        Body = mailBody.ToString(),
                        Subject = "Training Schedule",
                        ToEmail = unitHeadDetails.Email,
                    };
                    SendEmailAsync(UnitHeadmailPayload, null);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), new { Controller = "LearningAndDevelopmentMailService", Method = "SendTrainingScheduleNotificationMail" });
            }
        }
    }
}
