using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.Repositories.Departments.Repository;
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

namespace Com.XpressPayments.Data.Repositories.CountryStateLga
{
    public  class CountryRepository : ICountryRepository
    {
        private string _connectionString;
        private readonly ILogger<CountryRepository> _logger;
        private readonly IConfiguration _configuration;

        public CountryRepository(IConfiguration configuration, ILogger<CountryRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IEnumerable<CountryDTO>> GetAllCountries()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", CountryStateLgaEnum.GETALL);

                    var CountryDetails = await _dapper.QueryAsync<CountryDTO>(ApplicationConstant.Sp_get_countries, param: param, commandType: CommandType.StoredProcedure);

                    return CountryDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllCountries() ===>{ex.Message}");
                throw;
            }
        }
    }
}
