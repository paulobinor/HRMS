using Dapper;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace Com.XpressPayments.Data.Repositories.UserAccount.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private string _connectionString;
        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(IConfiguration configuration, ILogger<UnitOfWork> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public async Task<IEnumerable<SystemConfigDto>> SystemConfiguration()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.SYSTEMCONFIG);

                    var configData = await _dapper.QueryAsync<SystemConfigDto>(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return configData;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: SystemConfiguration() ===>{ ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdateRefreshToken(long userid, string email, string refreshToken)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.UPDREFRESHTOKEN);
                    param.Add("@UserIdRefT", userid);
                    param.Add("@UserEmailRefT", email);
                    param.Add("@RefreshToken", refreshToken);

                    dynamic resp = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return resp;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateRefreshToken(long userid, string email) ===>{ ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdateUserLoginActivity(long userId, string Ipaddress, string token)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.UPDLOGINACTIVITY);
                    param.Add("@UserIdLactivity", userId);
                    param.Add("@IpAddressLactivity", Ipaddress);
                    param.Add("@Token", token); 

                    dynamic rsp = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return rsp;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetApprovedTransactions() ===>{ ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdateLastLoginAttempt(int attemptCount, string OfficialMail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.UPDLOGINATTEMPT);
                    param.Add("@LoginFailedAttemptsCount", attemptCount); 
                    param.Add("@UserEmailLoginAttempt", OfficialMail);

                    dynamic rsp = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return rsp;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetApprovedTransactions() ===>{ ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdateLogout(string email)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.UPDLOGOUT);
                    param.Add("@UserEmailLogout", email);

                    dynamic rsp = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return rsp;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetApprovedTransactions() ===>{ ex.Message}");
                throw;
            }
        }
    }
}
