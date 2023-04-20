using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
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
using System.Collections;

namespace Com.XpressPayments.Data.Repositories.CountryStateLga
{
    public class LgaRepository : ILgaRepository
    {
        private string _connectionString;
        private readonly ILogger<LgaRepository> _logger;
        private readonly IConfiguration _configuration;

        public LgaRepository(IConfiguration configuration, ILogger<LgaRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IEnumerable<LgaDTO>> GetAllLga(long StateID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", CountryStateLgaEnum.GETALL);
                    param.Add("@StateIDGet", StateID);

                    var LgaDetails = await _dapper.QueryAsync<LgaDTO>(ApplicationConstant.Sp_get_lga, param: param, commandType: CommandType.StoredProcedure);

                    return LgaDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllLga() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable> GetLgaByStateId(long StateID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", CountryStateLgaEnum.GETBYID);
                    param.Add("@StateIDGet", StateID);
                    

                    var LgaDetails = await _dapper.QueryAsync<LgaDTO>(ApplicationConstant.Sp_get_lga, param: param, commandType: CommandType.StoredProcedure);

                    return LgaDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetLgaByLgaId(int StateID) ===>{ex.Message}");
                throw;
            }
        }
    }
}
