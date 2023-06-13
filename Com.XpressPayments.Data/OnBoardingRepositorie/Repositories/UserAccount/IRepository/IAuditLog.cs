using Com.XpressPayments.Data.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.UserAccount.IRepository
{
    public interface IAuditLog
    {
        Task<dynamic> LogActivity(AuditLogDto auditLog);
    }
}
