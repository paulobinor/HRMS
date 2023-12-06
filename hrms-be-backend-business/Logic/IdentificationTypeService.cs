using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.Repository;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        public async Task<ExecutedResult<string>> CreateIdentificationType(CreateIdentificationTypeDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(IdentificationTypeModulePrivilegeConstant.Create_Identification_Type, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{checkPrivilege}", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.IdentificationTypeName))
                {
                    isModelStateValidate = false;
                    validationMessage += "IdentificationType Name is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new ProcessIdentificationTypeReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    IdentificationTypeName = payload.IdentificationTypeName,
                    IsModifield = false,
                };
                string repoResponse = await _identificationTypeRepository.ProcessIdentificationType(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "CreateIdentificationType",
                    payload = JsonConvert.SerializeObject(payload),
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<string>() { responseMessage = "Created Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError($"IdentificationTypeService (CreateIdentificationType)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> UpdateIdentificationType(UpdateIdentificationTypeDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(IdentificationTypeModulePrivilegeConstant.Update_Identification_Type, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{checkPrivilege}", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.IdentificationTypeName))
                {
                    isModelStateValidate = false;
                    validationMessage += "IdentificationType Name is required";
                }
                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new ProcessIdentificationTypeReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    IdentificationTypeName = payload.IdentificationTypeName,
                    IsModifield = true,
                    IdentificationTypeId = payload.IdentificationTypeId,
                };
                string repoResponse = await _identificationTypeRepository.ProcessIdentificationType(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "UpdateIdentificationType",
                    payload = JsonConvert.SerializeObject(payload),
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<string>() { responseMessage = "Updated Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError($"IdentificationTypeService (UpdateIdentificationType)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
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
