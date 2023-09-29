using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace hrms_be_backend_data.Repository
{
    public class TaxRepository: ITaxRepository
    {
        private readonly ILogger<TaxRepository> _logger;
        private readonly IDapperGenericRepository _dapper;
        public TaxRepository(IConfiguration configuration, ILogger<TaxRepository> logger, IDapperGenericRepository dapper)
        {
            _logger = logger;
            _dapper = dapper;
        }
        public async Task<List<TaxPayableVm>> GetTaxPayable(long CompanyId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyId", CompanyId);
                return await _dapper.GetAll<TaxPayableVm>("sp_get_tax_payable", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"TaxRepository -> GetTaxPayable => {ex}");
                return new List<TaxPayableVm>();
            }

        }
    }
}
