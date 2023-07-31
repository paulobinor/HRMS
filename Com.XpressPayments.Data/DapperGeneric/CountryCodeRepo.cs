using Com.XpressPayments.Data.DapperGeneric;
using Com.XpressPayments.Data.DTOs;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using XpressHRMS.Data.IRepository;

namespace XpressHRMS.Data.Repository
{
    public class CountryCodeRepo : ICountryCodeRepo
    {
        private readonly ILogger<CountryCodeRepo> _logger;
        private readonly IDapperGenericRepository _dapperr;
        private readonly string _connectionString;

        public CountryCodeRepo(ILogger<CountryCodeRepo> logger, IConfiguration configuration, IDapperGenericRepository dapper)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("HRMSConnectionString");
            _dapperr = dapper;
        }

        public async Task<List<CountryDTO>> GetAllCountries()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    int d = (int)GetAllDefault.GetAll;
                    param.Add("@Status", ACTION.SELECTALL);
                    
                    var response = await _dapperr.GetAll<CountryDTO>("sp_get_countries", param, commandType: CommandType.StoredProcedure);
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
