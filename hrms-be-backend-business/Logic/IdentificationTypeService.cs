using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace hrms_be_backend_business.Logic
{
    public class IdentificationTypeService: IIdentificationTypeService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<IdentificationTypeService> _logger;
        private readonly IAccountRepository _accountRepository;        
        private readonly IUserAppModulePrivilegeRepository _privilegeRepository;      
        private readonly IAuthService _authService;       
        private readonly IMailService _mailService;      
        private readonly IIdentificationTypeRepository _identificationTypeRepository;

        public IdentificationTypeService(IAccountRepository accountRepository, ILogger<IdentificationTypeService> logger,
            IIdentificationTypeRepository identificationTypeRepository, IAuditLog audit, IUserAppModulePrivilegeRepository privilegeRepository, IAuthService authService, IMailService mailService)
        {
            _audit = audit;
            _logger = logger;
            _accountRepository = accountRepository;
            _identificationTypeRepository = identificationTypeRepository;          
            _authService = authService;           
            _mailService = mailService;          
            _privilegeRepository = privilegeRepository;           
        }
        public async Task<ExecutedResult<IEnumerable<IdenticationTypeVm>>> GetIdenticationTypes(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<IEnumerable<IdenticationTypeVm>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }

                var repoResponse = await _identificationTypeRepository.GetIdenticationType(accessUser.data.CompanyId);
                if (repoResponse == null)
                {
                    return new ExecutedResult<IEnumerable<IdenticationTypeVm>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }              

                return new ExecutedResult<IEnumerable<IdenticationTypeVm>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = repoResponse };
            }
            catch (Exception ex)
            {
                _logger.LogError($"IdentificationTypeService (GetIdenticationTypes)=====>{ex}");
                return new ExecutedResult<IEnumerable<IdenticationTypeVm>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
    }
}
