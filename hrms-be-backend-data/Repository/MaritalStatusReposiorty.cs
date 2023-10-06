using Dapper;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace hrms_be_backend_data.Repository
{
    public class MaritalStatusReposiorty : IMaritalStatusReposiorty
    {
        private string _connectionString;
        private readonly ILogger<MaritalStatusReposiorty> _logger;
        private readonly IConfiguration _configuration;

        public MaritalStatusReposiorty(IConfiguration configuration, ILogger<MaritalStatusReposiorty> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IEnumerable<MaritalStatusDTO>> GetAllMaritalStatus()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GetEnum.GETALL);

                    var MaritalStatusDetails = await _dapper.QueryAsync<MaritalStatusDTO>(ApplicationConstant.Sp_MaritalStatus, param: param, commandType: CommandType.StoredProcedure);

                    return MaritalStatusDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllMaritalStatus() ===>{ex.Message}");
                throw;
            }
        }
    }
}
