using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace hrms_be_backend_data.Repository
{
    public class CurrencyRepository: ICurrencyRepository
    {
        private readonly ILogger<CurrencyRepository> _logger;
        private readonly IDapperGenericRepository _dapper;
        public CurrencyRepository(IConfiguration configuration, ILogger<CurrencyRepository> logger, IDapperGenericRepository dapper)
        {
            _logger = logger;
            _dapper = dapper;
        }

        public async Task<string> ProcessCurrency(CurrencyReq payload)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CurrencyId", payload.CurrencyId);
                param.Add("@CurrencyName", payload.CurrencyName);
                param.Add("@CurrencyCode", payload.CurrencyCode);
                param.Add("@CurrencyLogo", payload.CurrencyLogo);
                param.Add("@IsActive", payload.IsActive);
                param.Add("@IsModifield", payload.IsModifield);
               
                return await _dapper.Get<string>("sp_process_currencies", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<List<CurrencyVm>> GetCurrencies()
        {
            try
            {
                var param = new DynamicParameters();

                return await _dapper.GetAll<CurrencyVm>("sp_get_currencies", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }
        public async Task<List<CurrencyVm>> GetCurrencyById(int Id)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                return await _dapper.GetAll<CurrencyVm>("sp_get_currency_by_id", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }
    }
}
