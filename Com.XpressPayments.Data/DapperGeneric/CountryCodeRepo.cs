using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;
using XpressHRMS.Data.Enums;
using XpressHRMS.Data.IRepository;
using XpressHRMS.IRepository;

namespace XpressHRMS.Data.Repository
{
    public class CountryCodeRepo : ICountryCodeRepo
    {
        private readonly ILogger<CountryCodeRepo> _logger;
        private readonly IDapperGeneric _dapperr;
        private readonly string _connectionString;

        public CountryCodeRepo(ILogger<CountryCodeRepo> logger, IConfiguration configuration, IDapperGeneric dapper)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("HRMSConnectionString");
            _dapperr = dapper;
        }

        public async Task<List<CountryCode>> GetAllCountries()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    int d = (int)GetAllDefault.GetAll;
                    param.Add("@Status", ACTION.SELECTALL);
                    
                    var response = await _dapperr.GetAll<CountryCode>("sp_get_countries", param, commandType: CommandType.StoredProcedure);
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }
    }
 }
