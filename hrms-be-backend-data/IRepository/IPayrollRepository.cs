using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public interface IPayrollRepository
    {
        Task<string> ProcessPayroll(PayrollReq payload);
        Task<string> RunPayroll(RunPayrollReq payload);
        Task<string> DeletePayroll(PayrollDeleteReq payload);
        Task<PayrollWithTotalVm> GetPayroll(long CompanyId, int PageNumber, int RowsOfPage, string SearchVal);
        Task<PayrollVm> GetPayrollById(long Id);
        Task<PayrollRunnedSummaryVm> GetPayrollRunnedSummary(long PayrollRunnedId);
        Task<PayrollRunnedDetailsWithTotalVm> GetPayrollRunnedDetails(long PayrollRunnedId, int PageNumber, int RowsOfPage);
        Task<PayrollRunnedWithTotalVm> GetPayrollRunned(long AccessByUserId, int PageNumber, int RowsOfPage);
        Task<PayrollRunnedWithTotalVm> GetPayrollRunnedForReport(long AccessByUserId, int PageNumber, int RowsOfPage, DateTime DateFrom, DateTime DateTo);
        Task<PayrollRunnedVm> GetPayrollRunnedById(long PayrollRunnedId);
        Task<List<PayrollCyclesVm>> GetPayrollCycles();
        Task<string> ProcessPayrollEarnings(PayrollEarningsReq payload);
        Task<string> DeletePayrollEarnings(PayrollEarningsDeleteReq payload);
        Task<List<PayrollEarningsVm>> GetPayrollEarnings(long PayrollId);

        Task<string> ProcessPayrollDeduction(PayrollDeductionReq payload);
        Task<string> DeletePayrollDeduction(PayrollDeductionDeleteReq payload);
        Task<List<PayrollDeductionsVm>> GetPayrollDeductions(long PayrollId);
        Task<List<PayrollDeductionComputationVm>> GetPayrollDeductionComputation(long DeductionId, long PayrollId);
        Task<List<PayrollRunnedReportVm>> GetPayrollRunnedReport(long PayRollRunnedId);
    }
}
