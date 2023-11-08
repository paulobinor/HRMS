using hrms_be_backend_common.Communication;
using hrms_be_backend_data.ViewModel;
using System.Security.Claims;

namespace hrms_be_backend_business.ILogic
{
    public interface IIdentificationTypeService
    {
        Task<ExecutedResult<IEnumerable<IdenticationTypeVm>>> GetIdenticationTypes(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
    }
}
