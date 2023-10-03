using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface IAccountRepository
    {
        Task<User> FindUser(string officialMail);
        Task<User> FindUser(long userId);
        Task<CreateUserResponse> AddUser(CreateUserDto user, int createdbyUserId, string createdbyuseremail);
        Task<dynamic> UpdateUser(UpdateUserDto user, int updatedbyUserId, string updatedbyuseremail);
        Task<dynamic> MapUserToDepartment(string email, long deptId, long CompId, int updatedbyUserId);

        Task<User> GetUserByToken(string Token);
        Task<User> GetUserById(long Id);
        Task<IEnumerable<User>> GetAllUsers();

        Task<IEnumerable<User>> GetAllActiveUsers();
        Task<IEnumerable<User>> GetAllUsersbyDeptId(long DeptId);
        Task<IEnumerable<User>> GetAllUsersbyRoleID(long RoleId);
        Task<IEnumerable<User>> GetAllUsersbyCompanyId(long companyId);
        Task<IEnumerable<User>> GetUsersPendingApproval(long CompanyId);
        Task<string> AuthenticateUser(string EmailAddress, int MaximumLoginAttempt, DateTime DateCreated);
        Task<string> VerifyUser(string Token, string LoggedInWithIPAddress, DateTime DateCreated);
        Task<dynamic> ApproveUser(long approvedByuserId, string defaultPass, string userEmail);
        Task<dynamic> DeclineUser(long disapprovedByuserId, string officialMail, string comment);
        Task<dynamic> DeactivateUser(long deactivatedByuserId, string userEmail, string comment);
        Task<dynamic> ReactivateUser(long reactivatedByuserId, string userEmail, string comment, string defaultpass);
        Task<dynamic> UnblockUser(long unblocedByuserId, string defaultPassword, string userEmail);
        Task<dynamic> ChangePassword(long userId, string newPassword);

        Task<User> GetUserByCompany(string OfficialMail, int companyId);

        void SendEmail(string recipientEmail, string firtname, string defaultPass, string subject, string wwwRootPath, string ip, string port, string appKey = null, string channel = null);

    }
}
