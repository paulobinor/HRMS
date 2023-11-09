using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace hrms_be_backend_business.Logic
{
    public class MaritalStatusService : IMaritalStatusService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<MaritalStatusService> _logger;

        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly IMaritalStatusReposiorty _MaritalStatusReposiorty;
        private readonly IAuthService _authService;

        public MaritalStatusService(IAccountRepository accountRepository, ILogger<MaritalStatusService> logger,
            IMaritalStatusReposiorty MaritalStatusReposiorty, IAuditLog audit, ICompanyRepository companyrepository, IAuthService authService)
        {
            _audit = audit;

            _logger = logger;

            _accountRepository = accountRepository;
            _MaritalStatusReposiorty = MaritalStatusReposiorty;
            _companyrepository = companyrepository;
            _authService = authService;
        }
        public async Task<ExecutedResult<IEnumerable<MaritalStatusDTO>>> GetAllMaritalStatus(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<IEnumerable<MaritalStatusDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var repoResponse = await _MaritalStatusReposiorty.GetAllMaritalStatus();
                return new ExecutedResult<IEnumerable<MaritalStatusDTO>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = repoResponse };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllMaritalStatus() ==> {ex.Message}");
                return new ExecutedResult<IEnumerable<MaritalStatusDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }

       
    }
}
