using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public interface IAccountRepository
    {

        Task<string> ProcessUser(CreateUserReq payload);
        Task<string> AuthenticateUser(string EmailAddress, int MaximumLoginAttempt, DateTime DateCreated);
        Task<string> VerifyUser(string Token, string LoggedInWithIPAddress, DateTime DateCreated);
        Task<dynamic> ApproveUser(long approvedByuserId, string defaultPass, string userEmail);
        Task<dynamic> DeclineUser(long disapprovedByuserId, string officialMail, string comment);
        Task<dynamic> DeactivateUser(long deactivatedByuserId, string userEmail, string comment);
        Task<dynamic> ReactivateUser(long reactivatedByuserId, string userEmail, string comment, string defaultpass);
        Task<dynamic> UnblockUser(long unblocedByuserId, string defaultPassword, string userEmail);
        Task<dynamic> ChangePassword(long userId, string newPassword);
        Task<string> UpdateRefreshToken(string RefreshToken, long UserId);

        Task<UserWithTotalVm> GetUsers(int PageNumber, int RowsOfPage);
        Task<UserWithTotalVm> GetUsersBackOffice(int PageNumber, int RowsOfPage);
        Task<UserWithTotalVm> GetUsersApprovedBackOffice(int PageNumber, int RowsOfPage);
        Task<UserWithTotalVm> GetUsersDisapprovedBackOffice(int PageNumber, int RowsOfPage);
        Task<UserWithTotalVm> GetUsersDeapctivatedBackOffice(int PageNumber, int RowsOfPage);
        Task<UserWithTotalVm> GetUsersByCompany(long CompanyId, int PageNumber, int RowsOfPage);
        Task<UserWithTotalVm> GetUsersApprovedByCompany(long CompanyId, int PageNumber, int RowsOfPage);
        Task<UserWithTotalVm> GetUsersDisapprovedByCompany(long CompanyId, int PageNumber, int RowsOfPage);
        Task<UserWithTotalVm> GetUsersDeactivatedByCompany(long CompanyId, int PageNumber, int RowsOfPage);
        Task<UserVm> GetUserById(long Id);
        Task<UserVm> GetUserByToken(string Token);
        Task<UserFullView> FindUser(long? UserId, string Email, string AccessToken);
        Task<UserFullView> FindUser(long? UserId);
        Task<UserFullView> FindUser(string Email);
        Task<UserVm> GetUserByEmployeeId(long EmployeeId);
        Task<List<UserModulesVm>> GetAppModulesAssigned(long UserId);

        Task<dynamic> MapUserToDepartment(string email, long deptId, long CompId, int updatedbyUserId);
    }
}
