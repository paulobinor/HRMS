using Dapper;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace hrms_be_backend_data.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private string _connectionString;
        private readonly ILogger<AccountRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDapperGenericRepository _dapper;
        public AccountRepository(IConfiguration configuration, ILogger<AccountRepository> logger, IDapperGenericRepository dapper)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
            _dapper = dapper;
        }

        public async Task<string> ProcessUser(CreateUserReq payload)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", payload.UserId);
                param.Add("@FirstName", payload.FirstName);
                param.Add("@MiddleName", payload.MiddleName);
                param.Add("@LastName", payload.LastName);
                param.Add("@OfficialMail", payload.OfficialMail);
                param.Add("@PhoneNumber", payload.PhoneNumber);
                param.Add("@PasswordHash", BCrypt.Net.BCrypt.HashPassword(payload.PasswordHash, BCrypt.Net.BCrypt.GenerateSalt()));
                param.Add("@UserStatusCode", payload.UserStatusCode);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                param.Add("@IsModifield", payload.IsModifield);

                return await _dapper.Get<string>("sp_process_user", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"AccountRepository => ProcessUser ===> {ex.Message}");
                throw;
            }
        }
        public async Task<string> UpdateRefreshToken(string RefreshToken, long UserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", UserId);
                param.Add("@RefreshToken", RefreshToken);

                return await _dapper.Get<string>("sp_update_refresh_token", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"AccountRepository => UpdateRefreshToken ===> {ex.Message}");
                throw;
            }
        }

        public async Task<string> AuthenticateUser(string EmailAddress, int MaximumLoginAttempt, DateTime DateCreated)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@EmailAddress", EmailAddress);
                param.Add("@MaximumLoginAttempt", MaximumLoginAttempt);
                param.Add("@DateCreated", DateCreated);

                return await _dapper.Get<string>("sp_authenticate_user", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> AuthenticateUser => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<string> VerifyUser(string Token, string LoggedInWithIPAddress, DateTime DateCreated)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Token", Token);
                param.Add("@LoggedInWithIPAddress", LoggedInWithIPAddress);
                param.Add("@DateCreated", DateCreated);

                return await _dapper.Get<string>("sp_verify_user", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> VerifyUser => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }


        public async Task<dynamic> ApproveUser(long approvedByuserId, string defaultPass, string userEmail)
        {
            try
            {
                var hashdefaultPassword = BCrypt.Net.BCrypt.HashPassword(defaultPass, BCrypt.Net.BCrypt.GenerateSalt());

                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.APPROVEUSER);
                    param.Add("@PasswordHashApprove", hashdefaultPassword);
                    param.Add("@UserIdApprove", approvedByuserId);
                    param.Add("@UserEmailApprove", userEmail);

                    dynamic resp = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return resp;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: ApproveUser(long approvedByuserId, string defaultPass, string userEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeclineUser(long disapprovedByuserId, string officialMail, string comment)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.DISAPPROVEUSER);
                    param.Add("@UserIdDisapprove", disapprovedByuserId);
                    param.Add("@UserEmailDisapprove", officialMail);
                    param.Add("@DisapprovedComment", comment);

                    dynamic resp = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return resp;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeclineUser(long disapprovedByuserId, string userEmail, string comment) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeactivateUser(long deactivatedByuserId, string OfficialMail, string comment)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.DEACTIVATEUSER);
                    param.Add("@DeactivatedByUserId", deactivatedByuserId);
                    param.Add("@UserEmailDeactivate", OfficialMail);
                    param.Add("@DeactivatedComment", comment);

                    dynamic resp = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return resp;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeactivateUser(long deactivatedByuserId, string userEmail, string comment) ===>{ex.Message}");
                throw;
            }
        }

        //public async Task<UserFullView> GetUserByEmail(string email)
        //{
        //    string query = @"select * from USers where OfficialMail = @Email and IsApproved = @IsApproved";
        //    var param = new DynamicParameters();
        //    param.Add("Email", email);
        //    param.Add("IsApproved", true);

        //    dynamic resp = await _dapper.Get<UserFullView>(query, param, commandType: CommandType.Text);

        //    return resp;

        //}

        public async Task<dynamic> ReactivateUser(long reactivatedByuserId, string OfficialMail, string comment, string defaultpass)
        {
            try
            {
                var hashdefaultPassword = BCrypt.Net.BCrypt.HashPassword(defaultpass, BCrypt.Net.BCrypt.GenerateSalt());

                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.REACTIVATEUSER);
                    param.Add("@PasswordHashReactivate", hashdefaultPassword);
                    param.Add("@ReactivatedByUserId", reactivatedByuserId);
                    param.Add("@UserEmailReactivate", OfficialMail);
                    param.Add("@ReactivatedComment", comment);

                    dynamic resp = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return resp;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: ReactivateUser(long reactivatedByuserId, string userEmail, string comment) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UnblockUser(long unblockedByuserId, string defaultPassword, string userEmail)
        {
            try
            {
                var hashdefaultPassword = BCrypt.Net.BCrypt.HashPassword(defaultPassword, BCrypt.Net.BCrypt.GenerateSalt());

                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.UNBLOCKUSER);
                    param.Add("@PasswordHashUnblock", hashdefaultPassword);
                    param.Add("@LastModifiedByUserIdUnblock", unblockedByuserId);
                    param.Add("@UserEmailToUnblock", userEmail);

                    dynamic resp = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return resp;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UnblockUser(long unblocedByuserId, string userEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> ChangePassword(long userId, string newPassword)
        {
            try
            {
                var hashNewPassword = BCrypt.Net.BCrypt.HashPassword(newPassword, BCrypt.Net.BCrypt.GenerateSalt());

                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.CHANGEPASSWORD);
                    param.Add("@PasswordHashModifr", hashNewPassword);
                    param.Add("@UserIdModifr", userId);

                    dynamic resp = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return resp;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: ChangePassword(long userId, string newPassword) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<UserWithTotalVm> GetUsers(int PageNumber, int RowsOfPage)
        {
            var returnData = new UserWithTotalVm();
            try
            {
                var param = new DynamicParameters();

                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                var result = await _dapper.GetMultiple("sp_get_users", param, gr => gr.Read<long>(), gr => gr.Read<UserVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<UserVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> GetUsers => {ex}");
                return returnData;
            }

        }
        public async Task<UserWithTotalVm> GetUsersBackOffice(int PageNumber, int RowsOfPage)
        {
            var returnData = new UserWithTotalVm();
            try
            {
                var param = new DynamicParameters();

                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                var result = await _dapper.GetMultiple("sp_get_users_back_office", param, gr => gr.Read<long>(), gr => gr.Read<UserVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<UserVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> GetUsersBackOffice => {ex}");
                return returnData;
            }

        }
        public async Task<UserWithTotalVm> GetUsersApprovedBackOffice(int PageNumber, int RowsOfPage)
        {
            var returnData = new UserWithTotalVm();
            try
            {
                var param = new DynamicParameters();

                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                var result = await _dapper.GetMultiple("sp_get_users_approved_back_office", param, gr => gr.Read<long>(), gr => gr.Read<UserVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<UserVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> GetUsersApprovedBackOffice => {ex}");
                return returnData;
            }

        }
        public async Task<UserWithTotalVm> GetUsersDisapprovedBackOffice(int PageNumber, int RowsOfPage)
        {
            var returnData = new UserWithTotalVm();
            try
            {
                var param = new DynamicParameters();

                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                var result = await _dapper.GetMultiple("sp_get_users_disapproved_back_office", param, gr => gr.Read<long>(), gr => gr.Read<UserVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<UserVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> GetUsersDisapprovedBackOffice => {ex}");
                return returnData;
            }

        }
        public async Task<UserWithTotalVm> GetUsersDeapctivatedBackOffice(int PageNumber, int RowsOfPage)
        {
            var returnData = new UserWithTotalVm();
            try
            {
                var param = new DynamicParameters();

                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                var result = await _dapper.GetMultiple("sp_get_users_deactivated_back_office", param, gr => gr.Read<long>(), gr => gr.Read<UserVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<UserVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> GetUsersDeapctivatedBackOffice => {ex}");
                return returnData;
            }

        }
        public async Task<UserWithTotalVm> GetUsersByCompany(long CompanyId, int PageNumber, int RowsOfPage)
        {
            var returnData = new UserWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyId", CompanyId);
                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                var result = await _dapper.GetMultiple("sp_get_users_by_company", param, gr => gr.Read<long>(), gr => gr.Read<UserVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<UserVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> GetUsersByCompany => {ex}");
                return returnData;
            }

        }
        public async Task<UserWithTotalVm> GetUsersApprovedByCompany(long CompanyId, int PageNumber, int RowsOfPage)
        {
            var returnData = new UserWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyId", CompanyId);
                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                var result = await _dapper.GetMultiple("sp_get_users_approved_by_company", param, gr => gr.Read<long>(), gr => gr.Read<UserVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<UserVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> GetUsersApprovedByCompany => {ex}");
                return returnData;
            }

        }
        public async Task<UserWithTotalVm> GetUsersDisapprovedByCompany(long CompanyId, int PageNumber, int RowsOfPage)
        {
            var returnData = new UserWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyId", CompanyId);
                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                var result = await _dapper.GetMultiple("sp_get_users_disapproved_by_company", param, gr => gr.Read<long>(), gr => gr.Read<UserVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<UserVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> GetUsersDisapprovedByCompany => {ex}");
                return returnData;
            }

        }
        public async Task<UserWithTotalVm> GetUsersDeactivatedByCompany(long CompanyId, int PageNumber, int RowsOfPage)
        {
            var returnData = new UserWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyId", CompanyId);
                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                var result = await _dapper.GetMultiple("sp_get_users_deactivated_by_company", param, gr => gr.Read<long>(), gr => gr.Read<UserVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<UserVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> GetUsersDeactivatedByCompany => {ex}");
                return returnData;
            }

        }

        public async Task<UserVm> GetUserById(long Id)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                return await _dapper.Get<UserVm>("sp_get_user_by_id", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository => GetUserById || {ex}");
                return new UserVm();
            }
        }
        public async Task<UserVm> GetUserByToken(string Token)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Token", Token);
                return await _dapper.Get<UserVm>("sp_get_user_by_token", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository => GetUserByToken || {ex}");
                return new UserVm();
            }
        }
        public async Task<UserFullView> FindUser(long? UserId, string Email, string AccessToken)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", UserId);
                param.Add("@Email", Email);
                param.Add("@AccessToken", AccessToken);
                return await _dapper.Get<UserFullView>("sp_find_user", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository => FindUser || {ex}");
                return new UserFullView();
            }
        }
        public async Task<UserFullView> FindUser(long? UserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", UserId);
                param.Add("@Email", null);
                param.Add("@AccessToken", null);
                return await _dapper.Get<UserFullView>("sp_find_user", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository => FindUser || {ex}");
                return new UserFullView();
            }
        }
        public async Task<UserFullView> FindUser(string Email)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", null);
                param.Add("@Email", Email);
                param.Add("@AccessToken", null);
                return await _dapper.Get<UserFullView>("sp_find_user", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository => FindUser || {ex}");
                return new UserFullView();
            }
        }
        public async Task<UserVm> GetUserByEmployeeId(long EmployeeId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", EmployeeId);
                return await _dapper.Get<UserVm>("sp_get_user_by_employee_id", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository => GetUserByEmployeeId || {ex}");
                return new UserVm();
            }
        }
        public async Task<List<UserModulesVm>> GetAppModulesAssigned(long UserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", UserId);
                return await _dapper.GetAll<UserModulesVm>("sp_get_app_modules_assigned", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository => GetUserAssigned || {ex}");
                return new List<UserModulesVm>();
            }
        }



        public async Task<dynamic> MapUserToDepartment(string email, long deptId, long CompId, int updatedbyUserId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.MAPUSERTOdEPT);
                    param.Add("@UserEmailMap", email);
                    param.Add("@DeptIdMap", deptId);
                    param.Add("@CompanyIdMap", CompId);
                    param.Add("@MapUpdatedByUserId", updatedbyUserId);

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: MapUserToDepartment(string email, long deptId, int updatedbyUserId) ===>{ex.Message}");
                throw;
            }
        }



    }
}
