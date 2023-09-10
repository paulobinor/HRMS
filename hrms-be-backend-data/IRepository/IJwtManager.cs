using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using System.Security.Claims;

namespace hrms_be_backend_data.IRepository
{
    public interface IJwtManager 
    {
        Task<AuthResponse> GenerateJsonWebToken(User user);
        Task<AuthResponse> RefreshJsonWebToken(long userId, Claim[] claims);
    }
}
