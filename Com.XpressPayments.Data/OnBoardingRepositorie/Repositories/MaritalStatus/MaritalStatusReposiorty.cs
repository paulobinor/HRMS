using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.Repositories.Gender;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.MaritalStatus
{
    public  class MaritalStatusReposiorty : IMaritalStatusReposiorty
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
