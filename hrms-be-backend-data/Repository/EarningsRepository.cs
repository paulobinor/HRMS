using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace hrms_be_backend_data.Repository
{
    public class EarningsRepository: IEarningsRepository
    {
        private readonly ILogger<EarningsRepository> _logger;     
        private readonly IDapperGenericRepository _dapper;
        public EarningsRepository(IConfiguration configuration, ILogger<EarningsRepository> logger, IDapperGenericRepository dapper)
        {          
            _logger = logger;
            _dapper = dapper;
        }

        public async Task<string> ProcessEarnings(EarningsReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@EarningsId", payload.EarningsId);
                param.Add("@EarningsName", payload.EarningsName);              
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                param.Add("@IsModification", payload.IsModification);                
                return await _dapper.Get<string>("sp_process_earnings", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"EarningsRepository -> ProcessEarnings => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }

        public async Task<string> DeleteEarnings(EarningsDeleteReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@EarningsId", payload.EarningsId);
                param.Add("@DeleteComment", payload.DeleteComment);              
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);              
                return await _dapper.Get<string>("sp_delete_earnings", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"EarningsRepository -> DeleteEarnings => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<EarningsVm> GetEarnings(long CompanyId)
        {           
            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyId", CompanyId);              
                return await _dapper.Get<EarningsVm>("sp_get_earnings", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EarningsRepository -> GetEarnings => {ex}");
                return new EarningsVm();
            }

        }
        public async Task<EarningsVm> GetEarningsById(long Id)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                return await _dapper.Get<EarningsVm>("sp_get_earnings_by_id", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EarningsRepository -> GetEarningsById => {ex}");
                return new EarningsVm();
            }

        }

      

        public async Task<string> ProcessEarningsComputation(EarningsComputationReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@EarningsId", payload.EarningsId);
                param.Add("@EarningsItemId", payload.EarningsItemId);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                param.Add("@IsDelete", payload.IsDelete);
                return await _dapper.Get<string>("sp_process_earnings_computation", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"EarningsRepository -> ProcessEarningsComputation => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<List<EarningsComputationVm>> GetEarningsComputation(long EarningsId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EarningsId", EarningsId);
                return await _dapper.GetAll<EarningsComputationVm>("sp_get_earnings_computations", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EarningsRepository -> GetEarningsComputation => {ex}");
                return new List<EarningsComputationVm>();
            }

        }
        public async Task<string> ProcessEarningsItem(EarningItemReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@EarningItemId", payload.EarningItemId);
                param.Add("@EarningsItemName", payload.EarningsItemName);
                param.Add("@CompanyId", payload.CompanyId);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                param.Add("@IsModification", payload.IsModification);
                return await _dapper.Get<string>("sp_process_earnings_item", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"EarningsRepository -> ProcessEarningsItem => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<string> DeleteEarningsItem(EarningsItemDeleteReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@EarningItemId", payload.EarningItemId);
                param.Add("@DeleteComment", payload.DeleteComment);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                return await _dapper.Get<string>("sp_delete_earnings_item", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"EarningsRepository -> DeleteEarningsItem => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<List<EarningsItemVm>> GetEarningsItem(long CompanyId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyId", CompanyId);
                return await _dapper.GetAll<EarningsItemVm>("sp_get_earnings_items", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EarningsRepository -> GetEarningsItem => {ex}");
                return new List<EarningsItemVm>();
            }

        }
        public async Task<EarningsItemVm> GetEarningsItemById(long Id)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                return await _dapper.Get<EarningsItemVm>("sp_get_earnings_items_by_id", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EarningsRepository -> GetEarningsItemById => {ex}");
                return new EarningsItemVm();
            }

        }

        public async Task<EarningsCRAVm> GetEarningsCRA(long CompanyId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyId", CompanyId);
                return await _dapper.Get<EarningsCRAVm>("sp_get_earning_cra", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EarningsRepository -> GetEarningsCRA => {ex}");
                return new EarningsCRAVm();
            }

        }
    }
}
