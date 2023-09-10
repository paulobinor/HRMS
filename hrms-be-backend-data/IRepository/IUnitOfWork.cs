using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
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
