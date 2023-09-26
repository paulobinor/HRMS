using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.Configuration;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using iText.StyledXmlParser.Jsoup.Helper;
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
    public class EarningsService : IEarningsService
    {
        private readonly ILogger<EarningsService> _logger;
        private readonly IEarningsRepository _earningsRepository;
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;
        private readonly JwtConfig _jwt;
        private readonly IAuditLog _audit;
        public EarningsService(IOptions<JwtConfig> jwt, ILogger<EarningsService> logger, IEarningsRepository earningsRepository, IAuditLog audit, IAuthService authService)
        {
            _logger = logger;
            _earningsRepository = earningsRepository;
            _jwt = jwt.Value;
            _audit = audit;
            _authService = authService;
        }
        public async Task<ExecutedResult<string>> CreateEarning(EarningsCreateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
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

                if (string.IsNullOrEmpty(payload.EarningsName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Earning name is required";
                }
                if (payload.EarningItems == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Earning Items is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new EarningsReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    EarningsName = payload.EarningsName,
                    IsModification = false,
                };
                string repoResponse = await _earningsRepository.ProcessEarnings(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }
                long earningId=Convert.ToInt64(repoResponse.Replace("Success",""));
                foreach(var item in payload.EarningItems)
                {
                    var earningCoputationPayload = new EarningsComputationReq
                    {
                        CreatedByUserId = accessUser.data.UserId,
                        DateCreated = DateTime.Now,
                        EarningsId = earningId,
                        EarningsItemId = item.EarningsItemId,
                        IsDelete = false
                    };
                   await _earningsRepository.ProcessEarningsComputation(earningCoputationPayload);
                }
              
                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "CreateEarning",
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
                _logger.LogError($"EarningsService (CreateEarning)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> UpdateEarning(EarningsCreateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
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

                if (string.IsNullOrEmpty(payload.EarningsName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Earning name is required";
                }
                if (payload.EarningItems == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Earning Items is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new EarningsReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    EarningsName = payload.EarningsName,
                    IsModification = false,
                };
                string repoResponse = await _earningsRepository.ProcessEarnings(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }
                long earningId = Convert.ToInt64(repoResponse.Replace("Success", ""));
                var earningItems = await _earningsRepository.GetEarningsComputation(earningId);
                foreach (var item in payload.EarningItems)
                {
                    bool isDeleted=false;
                    if (earningItems.Any(p => p.EarningsItemId == item.EarningsItemId))
                    {
                        isDeleted=true;
                    }
                    var earningCoputationPayload = new EarningsComputationReq
                    {
                        CreatedByUserId = accessUser.data.UserId,
                        DateCreated = DateTime.Now,
                        EarningsId = earningId,
                        EarningsItemId = item.EarningsItemId,
                        IsDelete = false
                    };
                    await _earningsRepository.ProcessEarningsComputation(earningCoputationPayload);
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "CreateEarning",
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
                _logger.LogError($"EarningsService (CreateEarning)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> DeleteEarnings(long EarningsId, string Comments, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
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
                //    return new ExecutedResult<string>() { responseMessage = validateAuthorization, responseCode = ((int)Id                //}
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(Comments))
                {
                    isModelStateValidate = false;
                    validationMessage += "Comments is required";
                }
                if (EarningsId<1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || EarningsId is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new EarningsDeleteReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                   EarningsId= EarningsId,
                   DeleteComment=Comments
                };
                string repoResponse = await _earningsRepository.DeleteEarnings(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }
               

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "DeleteEarnings",
                    payload = JsonConvert.SerializeObject(repoPayload),
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<string>() { responseMessage = "Deleted Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };


            }
            catch (Exception ex)
            {
                _logger.LogError($"EarningsService (CreateEarning)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<IEnumerable<EarningsView>>> GetEarnings(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<IEnumerable<EarningsView>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var result = await _earningsRepository.GetEarnings(accessUser.data.CompanyId);
                var earningsView = new List<EarningsView>();
                foreach (var item in result)
                {
                    var computations=await _earningsRepository.GetEarningsComputation(item.EarningsId);
                    earningsView.Add(new EarningsView
                    {
                        CreatedByUserId = item.CreatedByUserId,
                        DateCreated = item.DateCreated,
                        EarningsId = item.EarningsId,
                        EarningsName = item.EarningsName,
                        EarningsComputations = computations,
                    });
                }
                return new ExecutedResult<IEnumerable<EarningsView>>() { responseMessage = ((int)ResponseCode.Ok).ToString().ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = earningsView };
            }
            catch (Exception ex)
            {
                _logger.LogError($"EarningsService (GetEarnings)=====>{ex}");
                return new ExecutedResult<IEnumerable<EarningsView>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<EarningsView>> GetEarningsById(long Id, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<EarningsView>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var result = await _earningsRepository.GetEarningsById(Id);
               
                var computations = await _earningsRepository.GetEarningsComputation(result.EarningsId);
                var earningsView = new EarningsView
                {
                    CreatedByUserId = result.CreatedByUserId,
                    DateCreated = result.DateCreated,
                    EarningsId = result.EarningsId,
                    EarningsName = result.EarningsName,
                    EarningsComputations = computations,
                };
                return new ExecutedResult<EarningsView>() { responseMessage = ((int)ResponseCode.Ok).ToString().ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = earningsView };
            }
            catch (Exception ex)
            {
                _logger.LogError($"EarningsService (GetEarningsById)=====>{ex}");
                return new ExecutedResult<EarningsView>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }


        public async Task<ExecutedResult<string>> CreateEarningItem(EarningsItemCreateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            _logger.LogError($"EarningsService (AccessKey)=====>{AccessKey} || RemoteIpAddress {RemoteIpAddress}");
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

                if (string.IsNullOrEmpty(payload.EarningsItemName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Earning Item name is required";
                }
                

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new EarningItemReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    EarningsItemName = payload.EarningsItemName,
                    IsModification = false,
                    CompanyId = accessUser.data.CompanyId,
                    
                };
                string repoResponse = await _earningsRepository.ProcessEarningsItem(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }                

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "CreateEarningItem",
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
                _logger.LogError($"EarningsService (CreateEarningItem)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> UpdateEarningItem(EarningsItemUpdateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
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

                if (string.IsNullOrEmpty(payload.EarningsItemName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Earning Item name is required";
                }


                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new EarningItemReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    EarningsItemName = payload.EarningsItemName,
                    IsModification = true,
                    CompanyId = accessUser.data.CompanyId,
                    EarningItemId=payload.EarningsItemId

                };
                string repoResponse = await _earningsRepository.ProcessEarningsItem(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "UpdateEarningItem",
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
                _logger.LogError($"EarningsService (UpdateEarningItem)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> DeleteEarningsItem(long EarningsItemId, string Comments, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
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
                //    return new ExecutedResult<string>() { responseMessage = validateAuthorization, responseCode = ((int)Id                //}
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(Comments))
                {
                    isModelStateValidate = false;
                    validationMessage += "Comments is required";
                }
                if (EarningsItemId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || EarningsItemId is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new EarningsItemDeleteReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    EarningItemId = EarningsItemId,
                    DeleteComment = Comments
                };
                string repoResponse = await _earningsRepository.DeleteEarningsItem(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }


                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "DeleteEarningsItem",
                    payload = JsonConvert.SerializeObject(repoPayload),
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<string>() { responseMessage = "Deleted Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };


            }
            catch (Exception ex)
            {
                _logger.LogError($"EarningsService (CreateEarning)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<IEnumerable<EarningsItemVm>>> GetEarningsItem(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<IEnumerable<EarningsItemVm>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var result = await _earningsRepository.GetEarningsItem(accessUser.data.CompanyId);
                return new ExecutedResult<IEnumerable<EarningsItemVm>>() { responseMessage = ((int)ResponseCode.Ok).ToString().ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = result };
            }
            catch (Exception ex)
            {
                _logger.LogError($"EarningsService (GetEarningsItem)=====>{ex}");
                return new ExecutedResult<IEnumerable<EarningsItemVm>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<EarningsItemVm>> GetEarningsItemById(long Id, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<EarningsItemVm>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var result = await _earningsRepository.GetEarningsItemById(Id);

                return new ExecutedResult<EarningsItemVm>() { responseMessage = ((int)ResponseCode.Ok).ToString().ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = result };
            }
            catch (Exception ex)
            {
                _logger.LogError($"EarningsService (GetEarningsItemById)=====>{ex}");
                return new ExecutedResult<EarningsItemVm>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
    }
}
