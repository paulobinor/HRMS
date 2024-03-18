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
    public class AuditLogRepository : IAuditLog
    {
        private string _connectionString;
        private readonly ILogger<AuditLogRepository> _logger;

        public AuditLogRepository(IConfiguration configuration, ILogger<AuditLogRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public async Task<dynamic> LogActivity(AuditLogDto auditLog)
        {
            try
            {
                var hostname = System.Net.Dns.GetHostName();

                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var aParam = new DynamicParameters();
                    aParam.Add("@Status", Account.AUDIT);
                    aParam.Add("@UserId", auditLog.userId);
                    aParam.Add("@IpAddress", auditLog.ipAddress);
                    aParam.Add("@HostName", hostname);
                    aParam.Add("@ActionPerformed", auditLog.actionPerformed);
                    aParam.Add("@Payload", auditLog.payload);
                    aParam.Add("@Response", auditLog.response);
                    aParam.Add("@ActionStatus", auditLog.actionStatus);

                    var response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UserAuthandLogin, param: aParam, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: LogActivity(AuditLogDto auditLog) ===>" + ex.Message.ToString());
                return -1;
            }          
        }
    }
}
