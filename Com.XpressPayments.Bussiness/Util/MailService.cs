using Com.XpressPayments.Common.Configuration;
using Com.XpressPayments.Common.DTO.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Util
{
    public class MailService: IMailService
    {
        private readonly SmtpParameters _smtpParameters;
        private readonly ILogger<MailService> _logger;
        public MailService(IOptions<SmtpParameters> smtpParameters, ILogger<MailService> logger)
        {
            _smtpParameters = smtpParameters.Value;
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

    }
}
