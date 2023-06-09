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

namespace Com.XpressPayments.Data.Repositories.Institutions
{
    public  class InstitutionsRepository : IInstitutionsRepository
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
