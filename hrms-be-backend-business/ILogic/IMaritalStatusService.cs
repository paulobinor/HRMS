using hrms_be_backend_common.Communication;
using hrms_be_backend_data.RepoPayload;
using System.Security.Claims;

namespace hrms_be_backend_business.ILogic
{
    public  interface IMaritalStatusService
    {
        Task<ExecutedResult<IEnumerable<MaritalStatusDTO>>> GetAllMaritalStatus(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
    }
}
