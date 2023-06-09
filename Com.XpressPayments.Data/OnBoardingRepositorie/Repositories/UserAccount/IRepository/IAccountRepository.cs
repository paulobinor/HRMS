using Com.XpressPayments.Data.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.UserAccount.IRepository
{
    public interface IAccountRepository
    {
        Task<User> FindUser(string email);
        Task<User> FindUser(long userId);
        Task<CreateUserResponse> AddUser(CreateUserDto user, int createdbyUserId, string createdbyuseremail);
        Task<dynamic> UpdateUser(UpdateUserDto user, int updatedbyUserId, string updatedbyuseremail);
        Task<dynamic> MapUserToDepartment(string email, long deptId, long CompId, int updatedbyUserId);
        Task<IEnumerable<User>> GetAllUsers();

        Task<IEnumerable<User>> GetAllActiveUsers();
        Task<IEnumerable<User>> GetAllUsersbyDeptId(long DeptId);
        Task<IEnumerable<User>> GetAllUsersbyRoleID(long RoleId);
        Task<IEnumerable<User>> GetAllUsersbyCompanyId(long companyId);
        Task<IEnumerable<User>> GetUsersPendingApproval();
        Task<dynamic> ApproveUser(long approvedByuserId, string defaultPass, string userEmail);
        Task<dynamic> DeclineUser(long disapprovedByuserId, string userEmail, string comment);
        Task<dynamic> DeactivateUser(long deactivatedByuserId, string userEmail, string comment);
        Task<dynamic> ReactivateUser(long reactivatedByuserId, string userEmail, string comment, string defaultpass);
        Task<dynamic> UnblockUser(long unblocedByuserId, string defaultPassword, string userEmail);
        Task<dynamic> ChangePassword(long userId, string newPassword);

        void SendEmail(string recipientEmail, string firtname, string defaultPass, string subject, string wwwRootPath, string ip, string port, string appKey = null, string channel = null);

    }
}
