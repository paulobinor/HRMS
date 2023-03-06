using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.UserAccount.IRepository
{
    public interface IRefreshTokenGenerator
    {
        string GenerateRefreshToken();
    }
}
