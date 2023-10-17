using hrms_be_backend_data.ViewModel;
using System.Security.Claims;

namespace hrms_be_backend_data.IRepository
{
    public interface IJwtManager 
    {
        Task<AuthResponse> GenerateJsonWebToken(AccessUserVm user);
        Task<AuthResponse> RefreshJsonWebToken(long userId, Claim[] claims);
    }
}
