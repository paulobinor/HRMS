using Dapper;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;

namespace hrms_be_backend_data.Repository
{
    public class AccountRepository : IAccountRepository
    {
       
        private readonly ILogger<AccountRepository> _logger;      
        private readonly IDapperGenericRepository _dapper;
        public AccountRepository(ILogger<AccountRepository> logger, IDapperGenericRepository dapper)
        {           
            _logger = logger;          
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
        public async Task<string> CreateCompanyUser(CreateCompanyUserReq payload)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyId ", payload.CompanyId);
                param.Add("@FirstName", payload.FirstName);
                param.Add("@MiddleName", payload.MiddleName);
                param.Add("@LastName", payload.LastName);
                param.Add("@OfficialMail", payload.OfficialMail);
                param.Add("@PhoneNumber", payload.PhoneNumber);
                param.Add("@PasswordHash", BCrypt.Net.BCrypt.HashPassword(payload.PasswordHash, BCrypt.Net.BCrypt.GenerateSalt()));               
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);              

                return await _dapper.Get<string>("sp_create_company_user", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"AccountRepository => CreateCompanyUser ===> {ex.Message}");
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
        public async Task<string> ApproveUser(long Id, string defaultPassword, long CreatedByUserId, DateTime DateCreated)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                param.Add("@PasswordHashed", BCrypt.Net.BCrypt.HashPassword(defaultPassword, BCrypt.Net.BCrypt.GenerateSalt()));
                param.Add("@CreatedByUserId", CreatedByUserId);
                param.Add("@DateCreated", DateCreated);

                return await _dapper.Get<string>("sp_approve_user", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> ApproveUser => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<string> DeactivateUser(long Id, string Comment, long CreatedByUserId, DateTime DateCreated)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                param.Add("@Comment", Comment);
                param.Add("@CreatedByUserId", CreatedByUserId);
                param.Add("@DateCreated", DateCreated);

                return await _dapper.Get<string>("sp_delete_user", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> DeactivateUser => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<string> DisapproveUser(long Id, string Comment, long CreatedByUserId, DateTime DateCreated)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                param.Add("@Comment", Comment);
                param.Add("@CreatedByUserId", CreatedByUserId);
                param.Add("@DateCreated", DateCreated);

                return await _dapper.Get<string>("sp_disapprove_user", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> DisapproveUser => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<string> UnblockUser(long unblockedByuserId, string defaultPassword, string userEmail)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserEmail", userEmail);
                param.Add("@PasswordHashed", BCrypt.Net.BCrypt.HashPassword(defaultPassword, BCrypt.Net.BCrypt.GenerateSalt()));
                param.Add("@CreatedByUserId", unblockedByuserId);
                param.Add("@DateCreated", DateTime.Now);

                return await _dapper.Get<string>("sp_unblock_user", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> UnblockUser => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<string> LogoutUser(string EmailAddress)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmailAddress", EmailAddress);             

                return await _dapper.Get<string>("sp_log_out_user", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> LogoutUser => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<string> UpdateLoginActivity(long UserId, string IpAddress, string Token, DateTime DateCreated)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@UserId", UserId);
                param.Add("@IpAddress", IpAddress);
                param.Add("@Token", Token);
                param.Add("@DateCreated", DateCreated);
                return await _dapper.Get<string>("sp_update_login_activity", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> UpdateLoginActivity => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }

        public async Task<string> ChangePassword(long UserId, string defaultPassword, long CreatedByUserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", UserId);
                param.Add("@PasswordHashed", BCrypt.Net.BCrypt.HashPassword(defaultPassword, BCrypt.Net.BCrypt.GenerateSalt()));
                param.Add("@CreatedByUserId", CreatedByUserId);
                param.Add("@DateCreated", DateTime.Now);

                return await _dapper.Get<string>("sp_change_user_password", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> ChangePassword => {ex}");
                return "Unable to submit this detail, kindly contact support";
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
        
    }
}
