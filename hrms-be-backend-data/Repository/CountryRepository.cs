using Dapper;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace hrms_be_backend_data.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ILogger<CountryRepository> _logger;
        private readonly IDapperGenericRepository _dapper;

        public CountryRepository(IDapperGenericRepository dapper, ILogger<CountryRepository> logger)
        {
            _logger = logger;
            _dapper = dapper;
        }
       
        public async Task<List<CountryVm>> GetCountries()
        {
            try
            {
                var param = new DynamicParameters();              
                return await _dapper.GetAll<CountryVm>("sp_get_countries", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CountryRepository -> GetCountries => {ex}");
                return new List<CountryVm>();
            }
        }




        public async Task<List<CountryVm>> GetAllCountries()
        {
            try
            {
                string query = "Select * from Countries where IsActive = 1";
                var param = new DynamicParameters();
                return await _dapper.GetAll<CountryVm>(query, param, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CountryRepository -> GetAllCountries => {ex.ToString()}");
                return new List<CountryVm>();
            }
        }
    }
}
