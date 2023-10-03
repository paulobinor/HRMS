using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.ViewModel;
using System.Security.Claims;

namespace hrms_be_backend_business.ILogic
{
    public interface IEarningsService
    {
        Task<ExecutedResult<string>> CreateEarning(EarningsCreateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> UpdateEarning(EarningsUpdateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> DeleteEarnings(long EarningsId, string Comments, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<EarningsView>> GetEarnings(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);

        Task<ExecutedResult<string>> CreateEarningItem(EarningsItemCreateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> UpdateEarningItem(EarningsItemUpdateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> DeleteEarningsItem(long EarningsItemId, string Comments, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<IEnumerable<EarningsItemVm>>> GetEarningsItem(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<EarningsItemVm>> GetEarningsItemById(long Id, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
    }
}
