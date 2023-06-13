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
    public class StateRepository : IStateRepository
    {
        private string _connectionString;
        private readonly ILogger<StateRepository> _logger;
        private readonly IConfiguration _configuration;

        public StateRepository(IConfiguration configuration, ILogger<StateRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IEnumerable<StateDTO>> GetAllState(long CountryID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", CountryStateLgaEnum.GETALL);
                    param.Add("@CountryIDGet", CountryID);

                    var StateDetails = await _dapper.QueryAsync<StateDTO>(ApplicationConstant.Sp_get_states, param: param, commandType: CommandType.StoredProcedure);

                    return StateDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:  GetAllState() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable> GetStateByCountryId(long CountryID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", CountryStateLgaEnum.GETBYID);
                    param.Add("@CountryIDGet", CountryID);
                    //param.Add("@StateIDGet", StateID);

                    var StateDetails = await _dapper.QueryAsync<StateDTO>(ApplicationConstant.Sp_get_states, param: param, commandType: CommandType.StoredProcedure);

                    return StateDetails;
                }
            }


            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetStateByCountryId(int CountryID) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<StateDTO> GetStateByName(string StateName)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", CountryStateLgaEnum.GETBYNAME);
                    param.Add("@StateName", StateName);

                    var StateDetails = await _dapper.QueryFirstOrDefaultAsync<StateDTO>(ApplicationConstant.Sp_get_states, param: param, commandType: CommandType.StoredProcedure);

                    return StateDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:  GetStateByName(string StateName) ===>{ex.Message}");
                throw;
            }
        }
    }
}
