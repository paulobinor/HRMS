using Com.XpressPayments.Bussiness.ViewModels;
using Com.XpressPayments.Data.GenericResponse;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.UserAccount.IRepository
{
    public interface ITokenRefresher
    {
        Task<AuthResponse> Refresh(RefreshTokenModel request);
    }
}
