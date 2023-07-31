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
    public class StateCodeRepo : IStateCodeRepo
    {
        private readonly ILogger<StateCodeRepo> _logger;
        private readonly IDapperGeneric _dapperr;
        private readonly string _connectionString;

        public StateCodeRepo(ILogger<StateCodeRepo> logger, IConfiguration configuration, IDapperGeneric dapper)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("HRMSConnectionString");
            _dapperr = dapper;
        }

        public async Task<List<StateCodeDTO>> GetAllStateCountryID(int CountryID, int StateID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    int d = (int)GetAllDefault.GetAll;
                    param.Add("@Status", ACTION.SELECTBYID);
                    param.Add("@CountryIDGet", CountryID);
                    param.Add("@StateIDGet", StateID);
                    var response = await _dapperr.GetAll<StateCodeDTO>("sp_get_states", param, commandType: CommandType.StoredProcedure);
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<List<StateCodeDTO>> GetAllState(int CountryID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    int d = (int)GetAllDefault.GetAll;
                    param.Add("@Status", ACTION.SELECTALL);
                    param.Add("@CountryIDGet", CountryID);
                    //param.Add("@StateIDGet", StateID);
                    var response = await _dapperr.GetAll<StateCodeDTO>("sp_get_states", param, commandType: CommandType.StoredProcedure);
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
