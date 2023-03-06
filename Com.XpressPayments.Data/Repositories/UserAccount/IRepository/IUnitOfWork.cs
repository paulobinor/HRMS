using Com.XpressPayments.Data.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.UserAccount.IRepository
{
    public interface IUnitOfWork
    {
        Task<IEnumerable<SystemConfigDto>> SystemConfiguration();
        Task<dynamic> UpdateRefreshToken(long userid, string email, string refreshToken);
        Task<dynamic> UpdateUserLoginActivity(long userId, string Ipaddress, string token);
        Task<dynamic> UpdateLastLoginAttempt(int attemptCount, string Email);
        Task<dynamic> UpdateLogout(string email);
    }
}
