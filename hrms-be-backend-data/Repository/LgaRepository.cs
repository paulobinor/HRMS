using Dapper;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace hrms_be_backend_data.Repository
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


        public async Task<LgaDTO> GetLgaByName(string LGA_Name)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", CountryStateLgaEnum.GETBYNAME);
                    param.Add("@LGA_Name", LGA_Name);

                    var LgaDetails = await _dapper.QueryFirstOrDefaultAsync<LgaDTO>(ApplicationConstant.Sp_get_lga, param: param, commandType: CommandType.StoredProcedure);

                    return LgaDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:  GetLgaByName(string LGA_Name) ===>{ex.Message}");
                throw;
            }
        }
    }
}
