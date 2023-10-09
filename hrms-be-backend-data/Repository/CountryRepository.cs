using Dapper;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace hrms_be_backend_data.Repository
{
    public class CountryRepository : ICountryRepository
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

        public async Task<CountryDTO> GetCountryByName(string CountryName)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", CountryStateLgaEnum.GETBYNAME);
                    param.Add("@CountryNameGet", CountryName);

                    var CountryDetails = await _dapper.QueryFirstOrDefaultAsync<CountryDTO>(ApplicationConstant.Sp_get_countries, param: param, commandType: CommandType.StoredProcedure);

                    return CountryDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:  GetCountryByName(string CountryName) ===>{ex.Message}");
                throw;
            }
        }
    }
}
