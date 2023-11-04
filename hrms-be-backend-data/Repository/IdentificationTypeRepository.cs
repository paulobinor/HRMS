using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using System.Data;

namespace hrms_be_backend_data.Repository
{
    public class IdentificationTypeRepository: IIdentificationTypeRepository
    {
        private readonly ILogger<IdentificationTypeRepository> _logger;
        private readonly IDapperGenericRepository _dapper;

        public IdentificationTypeRepository(ILogger<IdentificationTypeRepository> logger, IDapperGenericRepository dapper)
        {
            _logger = logger;
            _dapper = dapper;
        }
        public async Task<List<IdenticationTypeVm>> GetIdenticationType(long CompanyId)
        {
            string query = "sp_get_identification_types";
            var param = new DynamicParameters();
            param.Add("CompanyId", CompanyId);        
            return await _dapper.GetAll<IdenticationTypeVm>(query, param, commandType: CommandType.StoredProcedure);

        }
    }
}
