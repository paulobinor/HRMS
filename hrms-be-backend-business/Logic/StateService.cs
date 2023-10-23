using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace hrms_be_backend_business.Logic
{
    public class StateService : IStateService
    {
        private readonly ILogger<StateService> _logger;
        private readonly IAuditLog _audit;
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly IStateRepository _stateRepository;
        private readonly IUserAppModulePrivilegeRepository _privilegeRepository;
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;
        private readonly IUriService _uriService;

        public StateService(IConfiguration configuration, IAccountRepository accountRepository, ILogger<StateService> logger,
            IStateRepository stateRepository, IAuditLog audit, IAuthService authService, IMailService mailService, IUriService uriService, IUserAppModulePrivilegeRepository privilegeRepository)
        {
            _audit = audit;

            _logger = logger;
            _configuration = configuration;
            _accountRepository = accountRepository;
            _stateRepository = stateRepository;
            _authService = authService;
            _mailService = mailService;
            _uriService = uriService;
            _privilegeRepository = privilegeRepository;
        }

        public async Task<ExecutedResult<List<StateVm>>> GetStates(int CountryId, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<List<StateVm>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var returnData = await _stateRepository.GetStates(CountryId);
                if (returnData == null)
                {
                    return new ExecutedResult<List<StateVm>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }
                return new ExecutedResult<List<StateVm>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = returnData };
            }
            catch (Exception ex)
            {
                _logger.LogError($"StateService (GetStates)=====>{ex}");
                return new ExecutedResult<List<StateVm>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
    }
}
