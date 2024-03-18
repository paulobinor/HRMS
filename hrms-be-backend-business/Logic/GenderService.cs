using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace hrms_be_backend_business.Logic
{
    public class GenderService : IGenderService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<GenderService> _logger;
        private readonly IAuthService _authService;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly IGenderRepository _GenderRepository;

        public GenderService(IAccountRepository accountRepository, ILogger<GenderService> logger,
            IGenderRepository GenderRepository, IAuditLog audit, ICompanyRepository companyrepository, IAuthService authService)
        {
            _audit = audit;

            _logger = logger;

            _accountRepository = accountRepository;
            _GenderRepository = GenderRepository;
            _companyrepository = companyrepository;
            _authService = authService;
        }

        public async Task<ExecutedResult<IEnumerable<GenderDTO>>> GetAllGender(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {           
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<IEnumerable<GenderDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var repoResponse = await _GenderRepository.GetAllGender();
                return new ExecutedResult<IEnumerable<GenderDTO>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = repoResponse };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllGender() ==> {ex.Message}");
                return new ExecutedResult<IEnumerable<GenderDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
    }
}
