using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using System.Data;

namespace hrms_be_backend_data.Repository
{
    public class StateRepository : IStateRepository
    {
        private readonly ILogger<StateRepository> _logger;
        private readonly IDapperGenericRepository _dapper;

        public StateRepository(IDapperGenericRepository dapper, ILogger<StateRepository> logger)
        {
            _logger = logger;
            _dapper = dapper;
        }

        public async Task<List<StateVm>> GetStates(int CountryId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CountryId", CountryId);
                return await _dapper.GetAll<StateVm>("sp_get_states", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"StateRepository -> GetStates => {ex}");
                return new List<StateVm>();
            }
        }
    }
}
