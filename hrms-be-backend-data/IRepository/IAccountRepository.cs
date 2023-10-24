using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public interface IAccountRepository
    {

        Task<string> ProcessUser(CreateUserReq payload);
        Task<string> CreateCompanyUser(CreateCompanyUserReq payload);
        Task<string> AuthenticateUser(string EmailAddress, int MaximumLoginAttempt, DateTime DateCreated);
        Task<string> VerifyUser(string Token, string LoggedInWithIPAddress, DateTime DateCreated);
        Task<string> ApproveUser(long Id, string defaultPassword, long CreatedByUserId, DateTime DateCreated);
        Task<string> DeactivateUser(long Id, string Comment, long CreatedByUserId, DateTime DateCreated);
        Task<string> UnblockUser(long unblockedByuserId, string defaultPassword, string userEmail);
        Task<string> UpdateRefreshToken(string RefreshToken, long UserId);
        Task<string> ChangePassword(long UserId, string defaultPassword, long CreatedByUserId);


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
    }
}
