using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public interface ITokenRefresher
    {
        Task<AuthResponse> Refresh(RefreshTokenModel request);
    }
}
