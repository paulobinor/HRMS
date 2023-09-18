using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.Configuration;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_business.Logic
{
    public class CurrencyService: ICurrencyService
    {
        private readonly ILogger<CurrencyService> _logger;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IAuthService _authService;     
        private readonly IMailService _mailService;
        private readonly JwtConfig _jwt;
        public CurrencyService(IOptions<JwtConfig> jwt, ILogger<CurrencyService> logger, ICurrencyRepository currencyRepository)
        {
            _logger = logger;
            _currencyRepository = currencyRepository;
            _jwt = jwt.Value;         
        }
        public async Task<ExecutedResult<string>> CreateBank(CurrencyCreateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, claim, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = accessUser.responseMessage, responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };
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
                    return new ExecutedResult<string>() { responseMessage = validationMessage, responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };
                }
                var repoPayload = new CurrencyReq() { CurrencyCode = payload.CurrencyCode, CurrencyName = payload.CurrencyName, IsActive =payload.IsActive, IsModifield = false};

                string result = await _bankRepository.ProcessBank(repoPayload);

                if (result.Contains("Success"))
                {

                    var auditry = new AuditTrailReq
                    {
                        AccessDate = DateTime.Now,
                        AccessedFromIpAddress = RemoteIpAddress,
                        AccessedFromPort = RemotePort,
                        UserId = accessUser.data.UserId,
                        Operation = "Bank Creation",
                        Payload = JsonConvert.SerializeObject(payload),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };
                    await _auditTrail.CreateAuditTrail(auditry);
                    int bankId = Convert.ToInt32(result.Replace("Success", ""));
                    var returnVal = await _bankRepository.GetByIdBank(bankId);


                    return new ExecutedResult<Banks>() { responseMessage = "Bank added successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = returnVal };
                }
                return new ExecutedResult<Banks>() { responseMessage = result, responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

            }
            catch (Exception ex)
            {
                _logger.LogError($"BankService (CreateBank)=====>{ex}");
                var msgBody = $"Hi, <br/> <br/> below is an exception detail occured <br/> <hr/>{ex}";
                _mailService.SendNotificationEmailAsync("Payout Exception", msgBody, AppOperationsData.Application_Operation_Management, XpressUserPriviledgeData.Checker, null);
                return new ExecutedResult<Banks>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<Banks>> UpdateBank(BankUpdateRequest payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _usersService.CheckUserAccess(AccessKey, claim);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<Banks>() { responseMessage = accessUser.responseMessage, responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };
                }
                var validateAuthorization = await _usersRepository.CheckXpressUserAuthorization(AppOperationsData.Bank_Management, XpressUserPriviledgeData.Maker, accessUser.data.UserId);
                if (!validateAuthorization.Contains("Success"))
                {
                    return new ExecutedResult<Banks>() { responseMessage = validateAuthorization, responseCode = ((int)ResponseCode.NO_PRIVILEDGE).ToString(), data = null };
                }
                bool isModelStateValidate = true;
                string validationMessage = "";
                if (AccessKey == null)
                {
                    isModelStateValidate = false;
                    validationMessage = "  || Access Key is required";
                }
                if (payload.BankId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage = "  || Bank Id must be greater than zero";
                }
                if (payload.BankName == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Bank is required";
                }
                if (payload.BankCode == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Bank Code is required";
                }
                if (payload.InstitutionCode == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Institution Code is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<Banks>() { responseMessage = validationMessage, responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };
                }
                var repoPayload = new BankReq() { BankCode = payload.BankCode, BankName = payload.BankName, CreatedByUserId = accessUser.data.UserId, DateCreated = DateTime.Now, BankId = payload.BankId, InstitutionCode = payload.InstitutionCode, IsModification = true };

                string result = await _bankRepository.ProcessBank(repoPayload);
                if (result.Contains("Success"))
                {

                    var auditry = new AuditTrailReq
                    {
                        AccessDate = DateTime.Now,
                        AccessedFromIpAddress = RemoteIpAddress,
                        AccessedFromPort = RemotePort,
                        UserId = accessUser.data.UserId,
                        Operation = "UpdateBank",
                        Payload = JsonConvert.SerializeObject(payload),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };
                    await _auditTrail.CreateAuditTrail(auditry);
                    int bankId = Convert.ToInt32(result.Replace("Success", ""));
                    var returnVal = await _bankRepository.GetByIdBank(bankId);


                    return new ExecutedResult<Banks>() { responseMessage = "Bank updated successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = returnVal };
                }
                return new ExecutedResult<Banks>() { responseMessage = result, responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

            }
            catch (Exception ex)
            {
                _logger.LogError($"BankService (UpdateBank)=====>{ex}");
                var msgBody = $"Hi, <br/> <br/> below is an exception detail occured <br/> <hr/>{ex}";
                _mailService.SendNotificationEmailAsync("Payout Exception", msgBody, AppOperationsData.Application_Operation_Management, XpressUserPriviledgeData.Checker, null);
                return new ExecutedResult<Banks>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<IEnumerable<Banks>>> GetAllBanks(string AccessKey, IEnumerable<Claim> claim)
        {
            try
            {
                var accessUser = await _usersService.CheckUserAccess(AccessKey, claim);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<IEnumerable<Banks>>() { responseMessage = accessUser.responseMessage, responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };
                }
                var result = await _bankRepository.GetAllBanks();
                return new ExecutedResult<IEnumerable<Banks>>() { responseMessage = ((int)ResponseCode.Ok).ToString().ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = result };
            }
            catch (Exception ex)
            {
                _logger.LogError($"BankService (GetAllBanks)=====>{ex}");
                var msgBody = $"Hi, <br/> <br/> below is an exception detail occured <br/> <hr/>{ex}";
                _mailService.SendNotificationEmailAsync("Payout Exception", msgBody, AppOperationsData.Application_Operation_Management, XpressUserPriviledgeData.Checker, null);
                return new ExecutedResult<IEnumerable<Banks>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<Banks>> GetByIdBank(int BankId, string AccessKey, IEnumerable<Claim> claim)
        {
            try
            {
                var accessUser = await _usersService.CheckUserAccess(AccessKey, claim);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<Banks>() { responseMessage = accessUser.responseMessage, responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };
                }
                var result = await _bankRepository.GetByIdBank(BankId);

                return new ExecutedResult<Banks>() { responseMessage = ((int)ResponseCode.Ok).ToString().ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = result };
            }
            catch (Exception ex)
            {
                _logger.LogError($"BankService (GetById)=====>{ex}");
                var msgBody = $"Hi, <br/> <br/> below is an exception detail occured <br/> <hr/>{ex}";
                _mailService.SendNotificationEmailAsync("Payout Exception", msgBody, AppOperationsData.Application_Operation_Management, XpressUserPriviledgeData.Checker, null);
                return new ExecutedResult<Banks>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
    }
}
