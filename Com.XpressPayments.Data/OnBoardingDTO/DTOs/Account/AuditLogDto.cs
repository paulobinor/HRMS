using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.DTOs.Account
{
    public class AuditLogDto
    {
        public long userId { get; set; }
        public string actionPerformed { get; set; }
        public string payload { get; set; }
        public string response { get; set; }
        public string actionStatus { get; set; }
        public string ipAddress { get; set; }
    }
}
