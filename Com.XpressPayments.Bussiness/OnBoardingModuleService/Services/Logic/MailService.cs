//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Com.XpressPayments.Bussiness.OnBoardingModuleService.Services.Logic;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using PayoutBeCommon.Configuration;
//using PayoutBeCommon.Request;
//using System.Net;
//using System.Net.Mail;
//using PayoutBeData.Repository;
//using System.Threading.Tasks;

//namespace Com.XpressPayments.Bussiness.OnBoardingModuleService.Services.Logic
//{


//    using Microsoft.Extensions.Logging;
//    using Microsoft.Extensions.Options;
//    using System.Net;
//    using System.Net.Mail;
//    using AutoMapper;
//    using MailKit;

//    namespace PayoutBeBusiness.Logic
//    {
//        public class MailService : IMailService
//        {
//            private readonly SmtpParameters _smtpParameters;
//            private readonly ILogger<MailService> _logger;
//            private readonly IInHouseOperationPriviledgeRepository _inHouseOperationPriviledgeRepository;
//            private readonly IMerchantUserAppOperationRepository _merchantUserAppOperationRepository;

//            public MailService(IOptions<SmtpParameters> smtpParameters, ILogger<MailService> logger, IInHouseOperationPriviledgeRepository inHouseOperationPriviledgeRepository, IMerchantUserAppOperationRepository merchantUserAppOperationRepository)
//            {
//                _smtpParameters = smtpParameters.Value;
//                _logger = logger;
//                _inHouseOperationPriviledgeRepository = inHouseOperationPriviledgeRepository;
//                _merchantUserAppOperationRepository = merchantUserAppOperationRepository;
//            }
//            public async Task SendEmailAsync(MailRequest mailRequest, string attarchDocument)
//            {
//                try
//                {
//                    MailMessage mail = new MailMessage();
//                    mail.From = new MailAddress(_smtpParameters.EmailFrom, _smtpParameters.DisplayName);
//                    mail.To.Add(mailRequest.ToEmail);
//                    mail.Subject = mailRequest.Subject;
//                    mail.Body = mailRequest.Body;
//                    Attachment attachment;
//                    if (attarchDocument != null)
//                    {
//                        attachment = new Attachment(attarchDocument);
//                        mail.Attachments.Add(attachment);
//                    }
//                    mail.IsBodyHtml = true;
//                    NetworkCredential Credentials = new NetworkCredential(_smtpParameters.Username, _smtpParameters.Password);
//                    using (var smtpClient = new SmtpClient(_smtpParameters.Host, _smtpParameters.Port))
//                    {
//                        smtpClient.Credentials = Credentials;
//                        smtpClient.EnableSsl = _smtpParameters.SSL;
//                        await smtpClient.SendMailAsync(mail);


//                    }
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError(ex.ToString(), new { Controller = "MailService", Method = "SendEmailAsync" });
//                }
//            }

//            public async Task SendNotificationEmailAsync(string MessageSubject, string MessageBody, string AppOperationCode, string PrivilegeCode, string attarchDocument)
//            {
//                try
//                {
//                    var listofEmail = await _inHouseOperationPriviledgeRepository.GetUserAppOperationPriviledgeEmails(AppOperationCode, PrivilegeCode);
//                    MailMessage mail = new MailMessage();
//                    mail.From = new MailAddress(_smtpParameters.EmailFrom, _smtpParameters.DisplayName);
//                    mail.Subject = MessageSubject;
//                    mail.Body = MessageBody;
//                    Attachment attachment;
//                    if (attarchDocument != null)
//                    {
//                        attachment = new Attachment(attarchDocument);
//                        mail.Attachments.Add(attachment);
//                    }
//                    mail.IsBodyHtml = true;
//                    //NetworkCredential Credentials = new NetworkCredential(_smtpParameters.Username, _smtpParameters.Password);
//                    //foreach (var email in listofEmail)
//                    //{                   
//                    //    mail.To.Add(email.EmailAddress);               
//                    //    using (var smtpClient = new SmtpClient(_smtpParameters.Host, _smtpParameters.Port))
//                    //    {
//                    //        smtpClient.Credentials = Credentials;
//                    //        smtpClient.EnableSsl = _smtpParameters.SSL;
//                    //        await smtpClient.SendMailAsync(mail);
//                    //    }
//                    //}
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError(ex.ToString(), new { Controller = "MailService", Method = "SendNotificationEmailAsync" });
//                }
//            }
//            public async Task SendNotificationEmailMerchantAsync(string MessageSubject, string MessageBody, string AppOperationCode, string PrivilegeCode, long MerchantId, string attarchDocument)
//            {
//                try
//                {
//                    var listofEmail = await _merchantUserAppOperationRepository.GetUserAppOperationPriviledgeEmailsMerchant(AppOperationCode, PrivilegeCode, MerchantId);
//                    MailMessage mail = new MailMessage();
//                    mail.From = new MailAddress(_smtpParameters.EmailFrom, _smtpParameters.DisplayName);
//                    mail.Subject = MessageSubject;
//                    mail.Body = MessageBody;
//                    Attachment attachment;
//                    if (attarchDocument != null)
//                    {
//                        attachment = new Attachment(attarchDocument);
//                        mail.Attachments.Add(attachment);
//                    }
//                    mail.IsBodyHtml = true;
//                    NetworkCredential Credentials = new NetworkCredential(_smtpParameters.Username, _smtpParameters.Password);
//                    foreach (var email in listofEmail)
//                    {
//                        mail.To.Add(email.EmailAddress);
//                        using (var smtpClient = new SmtpClient(_smtpParameters.Host, _smtpParameters.Port))
//                        {
//                            smtpClient.Credentials = Credentials;
//                            smtpClient.EnableSsl = _smtpParameters.SSL;
//                            await smtpClient.SendMailAsync(mail);
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError(ex.ToString(), new { Controller = "MailService", Method = "SendNotificationEmailMerchantAsync" });
//                }
//            }

//        }
//    }

//}
