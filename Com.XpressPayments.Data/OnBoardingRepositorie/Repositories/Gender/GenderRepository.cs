using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.Repositories.CountryStateLga;
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

namespace Com.XpressPayments.Data.Repositories.Gender
{
    public class GenderRepository : IGenderRepository
    {
        private string _connectionString;
        private readonly ILogger<GenderRepository> _logger;
        private readonly IConfiguration _configuration;

        public GenderRepository(IConfiguration configuration, ILogger<GenderRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IEnumerable<GenderDTO>> GetAllGender()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GetEnum.GETALL);

                    var GenderDetails = await _dapper.QueryAsync<GenderDTO>(ApplicationConstant.Sp_Gender, param: param, commandType: CommandType.StoredProcedure);

                    return GenderDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllGender() ===>{ex.Message}");
                throw;
            }
        }
    }
}
