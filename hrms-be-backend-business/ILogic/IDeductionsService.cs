using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.ViewModel;
using System.Security.Claims;

namespace hrms_be_backend_business.ILogic
{
    public interface IDeductionsService
    {
        Task<ExecutedResult<string>> CreateDeduction(DeductionCreateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> UpdateDeduction(DeductionUpdateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> DeleteDeduction(long DeductionId, string Comments, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<IEnumerable<DeductionView>>> GetDeductions(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<DeductionView>> GetDeductionById(long Id, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
    }
}
