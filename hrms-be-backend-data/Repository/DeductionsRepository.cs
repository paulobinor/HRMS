using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace hrms_be_backend_data.Repository
{
    public class DeductionsRepository: IDeductionsRepository
    {
        private readonly ILogger<DeductionsRepository> _logger;
        private readonly IDapperGenericRepository _dapper;
        public DeductionsRepository(IConfiguration configuration, ILogger<DeductionsRepository> logger, IDapperGenericRepository dapper)
        {
            _logger = logger;
            _dapper = dapper;
        }

        public async Task<string> ProcessDeductions(DeductionReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@DeductionId", payload.DeductionId);
                param.Add("@DeductionName", payload.DeductionName);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                param.Add("@IsModification", payload.IsModification);
                return await _dapper.Get<string>("sp_process_deduction", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"DeductionsRepository -> ProcessDeductions => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<string> DeleteDeduction(DeductionDeleteReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@DeductionId", payload.DeductionId);
                param.Add("@DeleteComment", payload.DeleteComment);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                return await _dapper.Get<string>("sp_delete_deduction", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"DeductionsRepository -> DeleteDeduction => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<List<DeductionVm>> GetDeduction(long CompanyId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyId", CompanyId);
                return await _dapper.GetAll<DeductionVm>("sp_get_deductions", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeductionsRepository -> GetDeduction => {ex}");
                return new List<DeductionVm>();
            }

        }
        public async Task<DeductionVm> GetDeductionById(long Id)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                return await _dapper.Get<DeductionVm>("sp_get_deductions_by_id", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeductionsRepository -> GetDeductionById => {ex}");
                return new DeductionVm();
            }

        }

        public async Task<string> ProcessDeductionComputation(DeductionComputationReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@DeductionId", payload.DeductionId);
                param.Add("@EarningsItemId", payload.EarningsItemId);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                param.Add("@IsDelete", payload.IsDelete);
                return await _dapper.Get<string>("sp_process_deduction_computation", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"DeductionRepository -> ProcessDeductionComputation => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<List<DeductionComputationVm>> GetDeductionComputation(long DeductionId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@DeductionId", DeductionId);
                return await _dapper.GetAll<DeductionComputationVm>("sp_get_deduction_computations", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeductionRepository -> GetDeductionComputation => {ex}");
                return new List<DeductionComputationVm>();
            }

        }
    }
}
