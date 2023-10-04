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
