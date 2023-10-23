using hrms_be_backend_common.Communication;
using hrms_be_backend_data.ViewModel;
using System.Security.Claims;

namespace hrms_be_backend_business.ILogic
{
    public interface ILgaService
    {
        Task<ExecutedResult<List<LgaVm>>> GetLgas(int StateId, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
    }
}
