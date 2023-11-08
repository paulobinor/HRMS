using hrms_be_backend_common.Communication;
using hrms_be_backend_data.RepoPayload;
using System.Security.Claims;

namespace hrms_be_backend_business.ILogic
{
    public  interface IGenderService
    {
        Task<ExecutedResult<IEnumerable<GenderDTO>>> GetAllGender(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
    }
}
