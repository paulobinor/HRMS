using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.Configuration;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;

namespace hrms_be_backend_business.Logic
{
    public class CurrencyService: ICurrencyService
    {
        private readonly ILogger<CurrencyService> _logger;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IAuthService _authService;     
        private readonly IMailService _mailService;
        private readonly JwtConfig _jwt;
        private readonly IAuditLog _audit;
        public CurrencyService(IOptions<JwtConfig> jwt, ILogger<CurrencyService> logger, ICurrencyRepository currencyRepository, IAuthService authService, IAuditLog audit)
        {
            _logger = logger;
            _currencyRepository = currencyRepository;
            _authService = authService;
            _jwt = jwt.Value;
            _audit = audit;
        }
        public async Task<ExecutedResult<string>> CreateCurrency(CurrencyCreateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
        
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };
                  
                }
                //var validateAuthorization = await _usersRepository.CheckXpressUserAuthorization(AppOperationsData.Bank_Management, XpressUserPriviledgeData.Maker, accessUser.data.UserId);
                //if (!validateAuthorization.Contains("Success"))
                //{
                //    return new ExecutedResult<string>() { responseMessage = validateAuthorization, responseCode = ((int)ResponseCode.NO_PRIVILEDGE).ToString(), data = null };
                //}
                bool isModelStateValidate = true;
                string validationMessage = "";
                
                if (string.IsNullOrEmpty(payload.CurrencyName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Currency name is required";
                }
                if (string.IsNullOrEmpty(payload.CurrencyCode))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Currency code is required";
                }              

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };
                              
                }
                var repoPayload = new CurrencyReq() { CurrencyCode = payload.CurrencyCode, CurrencyName = payload.CurrencyName, IsActive =payload.IsActive, IsModifield = false};

                string result = await _currencyRepository.ProcessCurrency(repoPayload);

                if (!result.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{result}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };                   
                }
                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "CreateCurrency",
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
                _logger.LogError($"CurrencyService (CreateCurrency)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };              
            }
        }
        public async Task<ExecutedResult<string>> UpdateCurrency(CurrencyUpdateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                //var validateAuthorization = await _usersRepository.CheckXpressUserAuthorization(AppOperationsData.Bank_Management, XpressUserPriviledgeData.Maker, accessUser.data.UserId);
                //if (!validateAuthorization.Contains("Success"))
                //{
                //    return new ExecutedResult<string>() { responseMessage = validateAuthorization, responseCode = ((int)ResponseCode.NO_PRIVILEDGE).ToString(), data = null };
                //}
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.CurrencyName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Currency name is required";
                }
                if (string.IsNullOrEmpty(payload.CurrencyCode))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Currency code is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new CurrencyReq() { CurrencyCode = payload.CurrencyCode, CurrencyName = payload.CurrencyName, IsActive = payload.IsActive, IsModifield = true,CurrencyId=payload.CurrencyId };

                string result = await _currencyRepository.ProcessCurrency(repoPayload);

                if (!result.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{result}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }
                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "UpdateCurrency",
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
                _logger.LogError($"CurrencyService (CreateCurrency)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<IEnumerable<CurrencyVm>>> GetCurrencies(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<IEnumerable<CurrencyVm>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var result = await _currencyRepository.GetCurrencies();
                return new ExecutedResult<IEnumerable<CurrencyVm>>() { responseMessage = ((int)ResponseCode.Ok).ToString().ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = result };
            }
            catch (Exception ex)
            {
                _logger.LogError($"CurrencyService (GetAllBanks)=====>{ex}");              
                return new ExecutedResult<IEnumerable<CurrencyVm>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<CurrencyVm>> GetCurrencyById(int Id, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<CurrencyVm>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var result = await _currencyRepository.GetCurrencyById(Id);

                return new ExecutedResult<CurrencyVm>() { responseMessage = ((int)ResponseCode.Ok).ToString().ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = result };
            }
            catch (Exception ex)
            {
                _logger.LogError($"CurrencyService (GetCurrencyById)=====>{ex}");              
                return new ExecutedResult<CurrencyVm>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
    }
}
