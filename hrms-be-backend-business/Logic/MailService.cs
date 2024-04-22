using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Configuration;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace hrms_be_backend_business.AppCode
{
    public class MailService : IMailService
    {
        private readonly SmtpConfig _smtpParameters;
        private readonly FrontendConfig _frontendConfig;
        private readonly ILogger<MailService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IHostingEnvironment _hostEnvironment;

        public MailService(IOptions<SmtpConfig> smtpParameters, IOptions<FrontendConfig> frontendConfig, ILogger<MailService> logger, IAccountRepository accountRepository, IHostingEnvironment hostEnvironment)
        {
            _smtpParameters = smtpParameters.Value;
            _frontendConfig = frontendConfig.Value;
            _accountRepository = accountRepository;
            _logger = logger;
            _hostEnvironment = hostEnvironment;

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
                _logger.LogError(ex.ToString(), new { Controller = "MailService", Method = "SendEmailAsync" });
            }
        }
        public async Task SendLeaveMailToReliever(long RelieverUserId, long leaveRequetedByUserId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var userDetails = await _accountRepository.FindUser(RelieverUserId, null, null);
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
                    ToEmail = userDetails.OfficialMail,
                };
                SendEmailAsync(mailPayload, null);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), new { Controller = "MailService", Method = "SendLeaveMailToReliever" });
            }
        }
        public async Task SendLeaveApproveMailToApprover(long ApprovalEmployeeId, long leaveRequetedByEmployeeId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var userDetails = await _accountRepository.GetUserByEmployeeId(ApprovalEmployeeId);
                var leaveRequetedBy = await _accountRepository.GetUserByEmployeeId(leaveRequetedByEmployeeId);
                StringBuilder mailBody = new StringBuilder();
                mailBody.Append($"Dear {userDetails.FirstName} {userDetails.LastName} {userDetails.MiddleName} <br/> <br/>");
                mailBody.Append($"Kindly login to approve a leave request by {leaveRequetedBy.FirstName} {leaveRequetedBy.LastName} {leaveRequetedBy.MiddleName} <br/> <br/>");
                mailBody.Append($"<b>Start Date : <b/> {startDate}  <br/> ");
                mailBody.Append($"<b>End Date : <b/> {endDate}   <br/> ");

                var mailPayload = new MailRequest
                {
                    Body = mailBody.ToString(),
                    Subject = "Leave Request",
                    ToEmail = userDetails.OfficialMail,
                };

                _logger.LogError($"Email payload to send: {mailPayload}.");
                SendEmailAsync(mailPayload, null);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), new { Controller = "MailService", Method = "SendLeaveMailToUnitHead" });
            }
        }
        public async Task SendLeaveApproveConfirmationMail(long RequesterEmployeeId, long ApprovedByEmployeeId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var userDetails = await _accountRepository.GetUserByEmployeeId(RequesterEmployeeId);
                var ApprovedByUserDetails = await _accountRepository.GetUserByEmployeeId(ApprovedByEmployeeId);
                StringBuilder mailBody = new StringBuilder();
                mailBody.Append($"Dear {userDetails.FirstName} {userDetails.LastName} {userDetails.MiddleName} <br/> <br/>");
                mailBody.Append($"You leave has been approved by {ApprovedByUserDetails.FirstName} {ApprovedByUserDetails.LastName} {ApprovedByUserDetails.MiddleName} <br/> <br/>");

                mailBody.Append($"<b> Your leave start from : <b/> {startDate} <br/> ");
                mailBody.Append($"<b>and ends on : <b/> {endDate} <br/> ");

                var mailPayload = new MailRequest
                {
                    Body = mailBody.ToString(),
                    Subject = "Leave Request",
                    ToEmail = userDetails.OfficialMail,
                };
                SendEmailAsync(mailPayload, null);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), new { Controller = "MailService", Method = "SendLeaveMailToUnitHead" });
            }
        }
        public async Task SendLeaveDisapproveConfirmationMail(long RequesterEmployeeId, long DiapprovedByEmployeeId)
        {
            try
            {
                var userDetails = await _accountRepository.GetUserByEmployeeId(RequesterEmployeeId);
                var disapprovedByUserDetails = await _accountRepository.GetUserByEmployeeId(DiapprovedByEmployeeId);

                StringBuilder mailBody = new StringBuilder();
                mailBody.Append($"Dear {userDetails.FirstName} {userDetails.LastName} {userDetails.MiddleName} <br/> <br/>");
                mailBody.Append($"You leave has been disapproved by {disapprovedByUserDetails.FirstName} {disapprovedByUserDetails.LastName} {disapprovedByUserDetails.MiddleName} <br/> <br/>");



                var mailPayload = new MailRequest
                {
                    Body = mailBody.ToString(),
                    Subject = "Leave Request",
                    ToEmail = userDetails.OfficialMail,
                };
                SendEmailAsync(mailPayload, null);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), new { Controller = "MailService", Method = "SendLeaveMailToUnitHead" });
            }
        }

        public async Task SendEmailApproveUser(string recipientEmail, string recipientName, string defaultPass, string subject, string token)
        {
            string emailAddress = _smtpParameters.EmailAddress;
            string smtpAdress = _smtpParameters.Host;
            int smtpPort = _smtpParameters.Port;
            string password = _smtpParameters.Password;

            string message = string.Empty;
            MimeMessage mailBody = new MimeMessage();

            MailboxAddress from = new MailboxAddress("Xpress HRMS", emailAddress);
            mailBody.From.Add(from);

            MailboxAddress to = new MailboxAddress(_smtpParameters.DisplayName, recipientEmail);
            //MailboxAddress to = new MailboxAddress("User", sampleEmail);
            mailBody.To.Add(to);

            mailBody.Subject = subject;
            message = ComposeApprovedUserMail(recipientName, token);
            BodyBuilder bodyBuilder = new BodyBuilder();
            _logger.LogInformation($"{subject} for {recipientEmail} with Message==> {message}");

            bodyBuilder.HtmlBody = message;
            mailBody.Body = bodyBuilder.ToMessageBody();

            try
            {
                MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
                client.Connect(smtpAdress, smtpPort);
                client.Authenticate(emailAddress, password);
                await client.SendAsync(mailBody);
                client.Disconnect(true);
                client.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: SendEmailApproveUser ===>{ex.Message}");
                throw;
            }
        }
        public void SendEmail(string recipientEmail, string firtname, string defaultPass, string subject, string wwwRootPath, string ip, string port, string appKey = null, string channel = null)
        {
            string emailAddress = _smtpParameters.EmailAddress;
            string smtpAdress = _smtpParameters.Host;
            int smtpPort = _smtpParameters.Port;
            string password = _smtpParameters.Password;
            //string sender = _configuration["Smtp:Sender"];

            //var sampleEmail = "yusufsunkanmi3@gmail.com";

            string message = string.Empty;
            MimeMessage mailBody = new MimeMessage();

            MailboxAddress from = new MailboxAddress("Xpress HRMS", emailAddress);
            mailBody.From.Add(from);

            MailboxAddress to = new MailboxAddress("User", recipientEmail);
            //MailboxAddress to = new MailboxAddress("User", sampleEmail);
            mailBody.To.Add(to);

            mailBody.Subject = subject;

            if (subject.ToLower().Contains("unblock"))
            {
                message = ComposeEmailToUnblockAccount(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
            }
            else if (subject.ToLower().Contains("password"))
            {
                message = ComposeEmailForPasswordChange(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
            }
            else if (subject.ToLower().Contains("re-activation"))
            {
                message = ComposeEmailForReactivationPasswordChange(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
            }
            else if (subject.ToLower().Contains("otp"))
            {
                message = ComposeEmailForOTP(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
            }
            else if (subject.ToLower().Contains("participation"))
            {
                message = ComposeEmailForSurveyParticipation(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
            }
            else if (subject.ToLower().Contains("longer"))
            {
                message = ComposeEmailForNoSurveyParticipation(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
            }
            else
            {
                message = ComposeSignUpMail(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
            }

            BodyBuilder bodyBuilder = new BodyBuilder();
            _logger.LogInformation($"{subject} for {recipientEmail} with Message==> {message}");

            bodyBuilder.HtmlBody = message;
            mailBody.Body = bodyBuilder.ToMessageBody();

            try
            {
                MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
                client.Connect(smtpAdress, smtpPort);
                client.Authenticate(emailAddress, password);
                client.Send(mailBody);
                client.Disconnect(true);
                client.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: SendEmail ===>{ex.Message}");
                throw;
            }
        }


        public string ComposeApprovedUserMail(string recipientName, string token)
        {

            string message = string.Empty;
            string body = string.Empty;
            string templatePath = string.Empty;

            string qryStr = string.Empty;
            string clientUrl = _frontendConfig.FrontendUrl;
            //string clientUrl = ip;
            //string clientUrl = $"http://{ip}:{port}/";
            templatePath = $"{_hostEnvironment.ContentRootPath}/EmailHandler/SignUp.html";
            qryStr = $"?k={token}";
            message = $"Dear {recipientName}," +
                      $"<p>You have been created on Xpress HRMS. Please click on the link below to activate your account</p>";

            using (StreamReader reader = new StreamReader(Path.Combine(templatePath)))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{link}", $"{clientUrl}{qryStr}");
            body = body.Replace("{MailContent}", message);

            return body;
        }
        public string ComposeSignUpMail(string firstname, string defaultPass, string email, string wwwRootPath, string ip, string port, string appKey = null, string channel = null)
        {

            string message = string.Empty;
            string body = string.Empty;
            string templatePath = string.Empty;

            if (null == channel)
            {
                string qryStr = string.Empty;
                string clientUrl = _frontendConfig.FrontendUrl;
                //string clientUrl = ip;
                //string clientUrl = $"http://{ip}:{port}/";
                templatePath = $"{wwwRootPath}/EmailHandler/SignUp.html";
                if (appKey == null)
                {
                    qryStr = $"?k={defaultPass}&a={email}";
                }
                else
                {
                    qryStr = $"?k={defaultPass}&a={email}&appkey={appKey}";
                }
                message = $"Dear {firstname}," +
                          $"<p>Thanks for registering on our platform. Please click on the link below to activate your account.</p>";

                using (StreamReader reader = new StreamReader(Path.Combine(templatePath)))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{link}", $"{clientUrl}{qryStr}");
                body = body.Replace("{MailContent}", message);

                return body;
            }
            else
            {
                templatePath = $"{wwwRootPath}/EmailHandler/SignUpWithToken.html";
                message = $"Dear {firstname}," +
                          $"<p>Thanks for registering on our platform. Use the following OTP to complete your Sign Up procedures.</p>";

                using (StreamReader reader = new StreamReader(Path.Combine(templatePath)))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{OTP}", defaultPass);
                body = body.Replace("{MailContent}", message);

                return body;
            }
        }
        public string ComposeEmailForPasswordChange(string firstname, string defaultPass, string email, string wwwRootPath, string ip, string port, string appKey = null, string channel = null)
        {

            string message = string.Empty;
            string body = string.Empty;
            string templatePath = string.Empty;

            if (null == channel)
            {
                string qryStr = string.Empty;
                string clientUrl = _frontendConfig.FrontendUrl;
                //string clientUrl = ip;
                //string clientUrl = $"http://{ip}:{port}/";
                templatePath = $"{wwwRootPath}/EmailHandler/ResetPassword.html";
                if (appKey == null)
                {
                    qryStr = $"?k={defaultPass}&a={email}";
                }
                else
                {
                    qryStr = $"?k={defaultPass}&a={email}&appkey={appKey}";
                }
                message = $"Dear {firstname}," +
                          $"<p>We recieved a password reset request for your account. Please click on the reset password button below to reset your account.</p>";

                using (StreamReader reader = new StreamReader(Path.Combine(templatePath)))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{link}", $"{clientUrl}{qryStr}");
                body = body.Replace("{MailContent}", message);

            }
            return body;
        }
        public string ComposeEmailForReactivationPasswordChange(string firstname, string defaultPass, string email, string wwwRootPath, string ip, string port, string appKey = null, string channel = null)
        {

            string message = string.Empty;
            string body = string.Empty;
            string templatePath = string.Empty;

            if (null == channel)
            {
                string qryStr = string.Empty;
                string clientUrl = _frontendConfig.FrontendUrl;
                //string clientUrl = ip;
                //string clientUrl = $"http://{ip}:{port}/";
                templatePath = $"{wwwRootPath}/EmailHandler/ResetPassword.html";
                if (appKey == null)
                {
                    qryStr = $"?k={defaultPass}&a={email}";
                }
                else
                {
                    qryStr = $"?k={defaultPass}&a={email}&appkey={appKey}";
                }
                message = $"Dear {firstname}," +
                          $"<p>Your account has been re-activted. Please click on the reset password button below to reset your account.</p>";

                using (StreamReader reader = new StreamReader(Path.Combine(templatePath)))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{link}", $"{clientUrl}{qryStr}");
                body = body.Replace("{MailContent}", message);

            }
            return body;
        }
        public string ComposeEmailForOTP(string firstname, string otp, string email, string wwwRootPath, string ip, string port, string appKey = null, string channel = null)
        {

            string message = string.Empty;
            string body = string.Empty;
            string templatePath = string.Empty;

            if (null == channel)
            {
                string qryStr = string.Empty;
                string clientUrl = _frontendConfig.FrontendUrl;
                //string clientUrl = ip;
                //string clientUrl = $"http://{ip}:{port}/";
                templatePath = $"{wwwRootPath}/EmailHandler/SendOtp.html";

                qryStr = $"?k={email}";

                message = $"Dear {firstname}," +
                          $"<p>Your request is now in progress. Your order confirmation OTP is : {otp}.</p>";

                using (StreamReader reader = new StreamReader(Path.Combine(templatePath)))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{link}", $"{otp}");
                body = body.Replace("{MailContent}", message);

            }
            return body;
        }

        public string ComposeEmailToUnblockAccount(string firstname, string defaultPass, string email, string wwwRootPath, string ip, string port, string appKey = null, string channel = null)
        {

            string message = string.Empty;
            string body = string.Empty;
            string templatePath = string.Empty;

            if (null == channel)
            {
                string qryStr = string.Empty;
                string clientUrl = _frontendConfig.FrontendUrl;
                templatePath = $"{wwwRootPath}/EmailHandler/ResetPassword.html";
                if (appKey == null)
                {
                    qryStr = $"?k={defaultPass}&a={email}";
                }
                else
                {
                    qryStr = $"?k={defaultPass}&a={email}&appkey={appKey}";
                }
                message = $"Dear {firstname}," +
                          $"<p>We recieved request to unblock your account. Please click on the reset password button below to reset your password and unbock your account.</p>";

                using (StreamReader reader = new StreamReader(Path.Combine(templatePath)))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{link}", $"{clientUrl}{qryStr}");
                body = body.Replace("{MailContent}", message);

            }
            return body;
        }


        public string ComposeEmailForSurveyParticipation(string firstname, string survProcessName, string email, string wwwRootPath, string ip, string port, string appKey = null, string channel = null)
        {

            string message = string.Empty;
            string body = string.Empty;
            string templatePath = string.Empty;

            if (null == channel)
            {
                string qryStr = string.Empty;
                string clientUrl = _frontendConfig.FrontendLoginUrl;
                //string clientUrl = ip;
                //string clientUrl = $"http://{ip}:{port}/";
                templatePath = $"{wwwRootPath}/EmailHandler/SurveyParticipation.html";

                qryStr = $"?k={email}";

                message = $"Dear {firstname}," +
                          $"<p>You have been added as a participant in Survey Name : {survProcessName}. Kindly login to the survey system to take survey.</p>";

                using (StreamReader reader = new StreamReader(Path.Combine(templatePath)))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{link}", $"{clientUrl}");
                body = body.Replace("{MailContent}", message);

            }
            return body;
        }

        public string ComposeEmailForNoSurveyParticipation(string firstname, string survProcessName, string email, string wwwRootPath, string ip, string port, string appKey = null, string channel = null)
        {

            string message = string.Empty;
            string body = string.Empty;
            string templatePath = string.Empty;

            if (null == channel)
            {
                string qryStr = string.Empty;
                string clientUrl = _frontendConfig.FrontendLoginUrl;
                //string clientUrl = ip;
                //string clientUrl = $"http://{ip}:{port}/";
                templatePath = $"{wwwRootPath}/EmailHandler/SurveyParticipationRemoved.html";

                qryStr = $"?k={email}";

                message = $"Dear {firstname}," +
                          $"<p>You are no longer a participant in Survey Name : {survProcessName}. Kindly contact HR for enquiries if any.</p>";

                using (StreamReader reader = new StreamReader(Path.Combine(templatePath)))
                {
                    body = reader.ReadToEnd();
                }

                //body = body.Replace("{link}", $"{clientUrl}");
                body = body.Replace("{MailContent}", message);

            }
            return body;
        }
    }
}
