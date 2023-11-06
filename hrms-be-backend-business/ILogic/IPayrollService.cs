using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.ViewModel;
using System.Security.Claims;

namespace hrms_be_backend_business.ILogic
{
    public interface IPayrollService
    {
        Task<ExecutedResult<string>> CreatePayroll(PayrollCreateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> UpdatePayroll(PayrollUpdateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> DeletePayroll(long PayrollId, string Comments, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> RunPayroll(RunPayrollDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);

        Task<PagedExcutedResult<IEnumerable<PayrollAllView>>> GetPayrolls(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<PayrollSingleView>> GetPayrollById(long Id, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<IEnumerable<PayrollCyclesVm>>> GetPayrollCycles(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<PayrollRunnedSummaryVm>> GetPayrollRunnedSummary(long PayrollRunnedId, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<PagedExcutedResult<IEnumerable<PayrollRunnedDetailsVm>>> GetPayrollRunnedDetails(PaginationFilter filter, long PayrollRunnedId, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
    }
}
