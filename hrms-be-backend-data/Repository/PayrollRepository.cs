using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.Design;
using System.Data;

namespace hrms_be_backend_data.Repository
{
    public class PayrollRepository: IPayrollRepository
    {
        private readonly ILogger<PayrollRepository> _logger;
        private readonly IDapperGenericRepository _dapper;
        public PayrollRepository(IConfiguration configuration, ILogger<PayrollRepository> logger, IDapperGenericRepository dapper)
        {
            _logger = logger;
            _dapper = dapper;
        }

        public async Task<string> ProcessPayroll(PayrollReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@PayrollId", payload.PayrollId);
                param.Add("@PayrollTitle", payload.PayrollTitle);
                param.Add("@PayrollDescription", payload.PayrollDescription);
                param.Add("@CurrencyId", payload.CurrencyId);
                param.Add("@PayrollCycleId", payload.PayrollCycleId);
                param.Add("@Payday", payload.Payday);
                param.Add("@PaydayLastDayOfTheCycle", payload.PaydayLastDayOfTheCycle);
                param.Add("@PaydayLastWeekOfTheCycle", payload.PaydayLastWeekOfTheCycle);
                param.Add("@PaydayCustomDayOfTheCycle", payload.PaydayCustomDayOfTheCycle);
                param.Add("@ProrationPolicy", payload.ProrationPolicy);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                param.Add("@IsModification", payload.IsModification);
                return await _dapper.Get<string>("sp_process_payroll", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollRepository -> ProcessPayroll => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<string> RunPayroll(RunPayrollReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@PayrollId", payload.PayrollId);
                param.Add("@Title", payload.Title);               
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);             
                return await _dapper.Get<string>("sp_run_payroll", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollRepository -> RunPayroll => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<PayrollRunnedSummaryVm> GetPayrollRunnedSummary(long PayrollRunnedId)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@PayrollRunnedId", PayrollRunnedId);               
                return await _dapper.Get<PayrollRunnedSummaryVm>("sp_get_payroll_runned_summary", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollRepository -> GetPayrollRunnedSummary => {ex}");
                return null;
            }

        }
        public async Task<PayrollRunnedDetailsWithTotalVm> GetPayrollRunnedDetails(long PayrollRunnedId, int PageNumber, int RowsOfPage)
        {
            var returnData = new PayrollRunnedDetailsWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@PayrollRunnedId", PayrollRunnedId);
                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                var result = await _dapper.GetMultiple("sp_get_payroll_runned_details", param, gr => gr.Read<long>(), gr => gr.Read<PayrollRunnedDetailsVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<PayrollRunnedDetailsVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollRepository -> GetPayrollRunnedDetails => {ex}");
                return returnData;
            }

        }
        public async Task<PayrollRunnedWithTotalVm> GetPayrollRunned(long AccessByUserId, int PageNumber, int RowsOfPage)
        {
            var returnData = new PayrollRunnedWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@AccessByUserId", AccessByUserId);
                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                var result = await _dapper.GetMultiple("sp_get_payroll_runned", param, gr => gr.Read<long>(), gr => gr.Read<PayrollRunnedVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<PayrollRunnedVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollRepository -> GetPayrollRunned => {ex}");
                return returnData;
            }

        }

        public async Task<string> DeletePayroll(PayrollDeleteReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@PayrollId", payload.PayrollId);
                param.Add("@DeleteComment", payload.DeleteComment);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                return await _dapper.Get<string>("sp_delete_payroll", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollRepository -> DeletePayroll => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<PayrollWithTotalVm> GetPayroll(long CompanyId, int PageNumber, int RowsOfPage)
        {
            var returnData = new PayrollWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyId", CompanyId);
                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                var result = await _dapper.GetMultiple("sp_get_payrolls", param, gr => gr.Read<long>(), gr => gr.Read<PayrollVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<PayrollVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;
               
            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollRepository -> GetPayroll => {ex}");
                return returnData;
            }

        }
        public async Task<PayrollVm> GetPayrollById(long Id)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                return await _dapper.Get<PayrollVm>("sp_get_payroll_by_id", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollRepository -> GetPayrollById => {ex}");
                return new PayrollVm();
            }

        }

        public async Task<string> ProcessPayrollEarnings(PayrollEarningsReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@PayrollId", payload.PayrollId);
                param.Add("@EarningsItemId", payload.EarningsItemId);              
                param.Add("@EarningsItemAmount", payload.EarningsItemAmount);               
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);              
                return await _dapper.Get<string>("sp_process_payroll_earnings", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollRepository -> ProcessPayrollEarnings => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<string> DeletePayrollEarnings(PayrollEarningsDeleteReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@PayrollId", payload.PayrollId);
                param.Add("@EarningsItemId", payload.EarningsItemId);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                return await _dapper.Get<string>("sp_delete_payroll_earnings", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollRepository -> DeletePayrollEarnings => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<List<PayrollEarningsVm>> GetPayrollEarnings(long PayrollId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@PayrollId", PayrollId);
                return await _dapper.GetAll<PayrollEarningsVm>("sp_get_payroll_earnings", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollRepository -> GetPayrollEarnings => {ex}");
                return new List<PayrollEarningsVm>();
            }

        }

        public async Task<string> ProcessPayrollDeduction(PayrollDeductionReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@PayrollId", payload.PayrollId);
                param.Add("@DeductionId", payload.DeductionId);
                param.Add("@IsFixed", payload.IsFixed);                      
                param.Add("@DeductionFixedAmount", payload.DeductionFixedAmount);
                param.Add("@IsPercentage", payload.IsPercentage);
                param.Add("@DeductionPercentageAmount", payload.DeductionPercentageAmount);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                return await _dapper.Get<string>("sp_process_payroll_deduction", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollRepository -> ProcessPayrollDeduction => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<string> DeletePayrollDeduction(PayrollDeductionDeleteReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@PayrollId", payload.PayrollId);
                param.Add("@DeductionId", payload.DeductionId);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                return await _dapper.Get<string>("sp_delete_payroll_deduction", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollRepository -> DeletePayrollDeduction => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<List<PayrollDeductionsVm>> GetPayrollDeductions(long PayrollId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@PayrollId", PayrollId);
                return await _dapper.GetAll<PayrollDeductionsVm>("sp_get_payroll_deductions", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollRepository -> GetPayrollDeductions => {ex}");
                return new List<PayrollDeductionsVm>();
            }

        }

        public async Task<List<PayrollCyclesVm>> GetPayrollCycles()
        {
            try
            {
                var param = new DynamicParameters();            
                return await _dapper.GetAll<PayrollCyclesVm>("sp_get_payroll_cycles", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollRepository -> GetPayrollCycles => {ex}");
                return new List<PayrollCyclesVm>();
            }

        }       
    }

}
