using Com.XpressPayments.Common.DTO.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Util
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest, string attarchDocument);
    }
}
