using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
using System;
using System.Security.Cryptography;

namespace Com.XpressPayments.Data.Repositories.UserAccount.Repository
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        public string GenerateRefreshToken()
        {
            var randomNumber = new Byte[32];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
