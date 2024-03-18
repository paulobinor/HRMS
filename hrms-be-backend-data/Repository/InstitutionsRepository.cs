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
    public class InstitutionsRepository : IInstitutionsRepository
    {
        private string _connectionString;
        private readonly ILogger<InstitutionsRepository> _logger;
        private readonly IConfiguration _configuration;

        public InstitutionsRepository(IConfiguration configuration, ILogger<InstitutionsRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IEnumerable<InstitutionsDTO>> GetAllInstitutions()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GetEnum.GETALL);

                    var InstitutionsDetails = await _dapper.QueryAsync<InstitutionsDTO>(ApplicationConstant.Sp_Institutions, param: param, commandType: CommandType.StoredProcedure);

                    return InstitutionsDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllInstitutions() ===>{ex.Message}");
                throw;
            }
        }
    }
}
