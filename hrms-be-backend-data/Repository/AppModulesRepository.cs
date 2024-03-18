using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using System.Data;

namespace hrms_be_backend_data.Repository
{
    public class AppModulesRepository: IAppModulesRepository
    {
        private readonly ILogger<AppModulesRepository> _logger;
        private readonly IDapperGenericRepository _dapper;

        public AppModulesRepository(ILogger<AppModulesRepository> logger, IDapperGenericRepository dapper)
        {
            _logger = logger;
            _dapper = dapper;
        }
        public async Task<List<AppModulesVm>> GetAppModules()
        {
            try
            {
                var param = new DynamicParameters();              

                return await _dapper.GetAll<AppModulesVm>("sp_get_app_modules", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyRepository -> GetAppModules => {ex}");
                return null;
            }

        }
    }
}
