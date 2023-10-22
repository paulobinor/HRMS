using Dapper;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace hrms_be_backend_data.Repository
{
    public class LgaRepository : ILgaRepository
    {

        private readonly ILogger<LgaRepository> _logger;
        private readonly IDapperGenericRepository _dapper;

        public LgaRepository(IDapperGenericRepository dapper, ILogger<LgaRepository> logger)
        {
            _logger = logger;
            _dapper = dapper;
        }

        public async Task<List<LgaVm>> GetLgas(int StateId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@StateId", StateId);
                return await _dapper.GetAll<LgaVm>("sp_get_lga", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"LgaRepository -> GetLgas => {ex}");
                return new List<LgaVm>();
            }
        }
    }
}
