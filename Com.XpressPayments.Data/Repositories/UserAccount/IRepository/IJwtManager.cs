using Com.XpressPayments.Data.DTOs.Account;
using Com.XpressPayments.Data.GenericResponse;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.UserAccount.IRepository
{
    public interface IJwtManager
    {
        Task<AuthResponse> GenerateJsonWebToken(User user);
        Task<AuthResponse> RefreshJsonWebToken(long userId, Claim[] claims);
    }
}
