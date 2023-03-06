using System;
using System.Collections.Generic;
using System.Text;

namespace Com.XpressPayments.Bussiness.ViewModels
{
    public class EmailModel
    {
        public string senderName { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
        public Array recipients { get; set; }
        public string status { get; set; }
        public string messageId { get; set; }
        public string transactionId { get; set; }
        public string sender { get; set; }

    }
}
