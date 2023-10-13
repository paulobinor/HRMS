using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public interface IPayrollRepository
    {
        Task<string> ProcessPayroll(PayrollReq payload);
        Task<string> DeletePayroll(PayrollDeleteReq payload);
        Task<PayrollWithTotalVm> GetPayroll(long CompanyId, int PageNumber, int RowsOfPage);
        Task<PayrollVm> GetPayrollById(long Id);


        Task<List<PayrollCyclesVm>> GetPayrollCycles();

        Task<string> ProcessPayrollEarnings(PayrollEarningsReq payload);
        Task<string> DeletePayrollEarnings(PayrollEarningsDeleteReq payload);
        Task<List<PayrollEarningsVm>> GetPayrollEarnings(long PayrollId);

        Task<string> ProcessPayrollDeduction(PayrollDeductionReq payload);
        Task<string> DeletePayrollDeduction(PayrollDeductionDeleteReq payload);       
        Task<List<PayrollDeductionsVm>> GetPayrollDeductions(long PayrollId);
    }
}
