using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.Configuration;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.Repository;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;

namespace hrms_be_backend_business.Logic
{
    public class DeductionsService: IDeductionsService
    {
        private readonly ILogger<DeductionsService> _logger;
        private readonly IDeductionsRepository _deductionsRepository;
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;
        private readonly JwtConfig _jwt;
        private readonly IAuditLog _audit;
        public DeductionsService(IOptions<JwtConfig> jwt, ILogger<DeductionsService> logger, IDeductionsRepository deductionsRepository, IAuditLog audit, IAuthService authService)
        {
            _logger = logger;
            _deductionsRepository = deductionsRepository;
            _jwt = jwt.Value;
            _audit = audit;
            _authService = authService;
        }
        public async Task<ExecutedResult<string>> CreateDeduction(DeductionCreateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }               
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.DeductionName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Deduction name is required";
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
                var repoPayload = new DeductionReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    DeductionName = payload.DeductionName,
                    IsModification = false,
                };
                string repoResponse = await _deductionsRepository.ProcessDeductions(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }
                long deductionId = Convert.ToInt64(repoResponse.Replace("Success", ""));
                foreach (var item in payload.EarningItems)
                {
                    var earningCoputationPayload = new DeductionComputationReq
                    {
                        CreatedByUserId = accessUser.data.UserId,
                        DateCreated = DateTime.Now,
                        DeductionId = deductionId,
                        EarningsItemId = item.EarningsItemId,
                        IsDelete = false
                    };
                    await _deductionsRepository.ProcessDeductionComputation(earningCoputationPayload);
                }
                var deductions = await _deductionsRepository.GetDeductionComputation(deductionId);
                foreach (var deductionComputation in deductions)
                {
                    var checkEarningItem = payload.EarningItems.Where(p => p.EarningsItemId == deductionComputation.EarningsItemId).ToList();
                    if (checkEarningItem.Count < 1)
                    {
                        var earningCoputationPayload = new DeductionComputationReq
                        {
                            CreatedByUserId = accessUser.data.UserId,
                            DateCreated = DateTime.Now,
                            DeductionId = deductionId,
                            EarningsItemId = deductionComputation.EarningsItemId,
                            IsDelete = true
                        };
                        await _deductionsRepository.ProcessDeductionComputation(earningCoputationPayload);
                    }
                }
                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "CreateDeduction",
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
                _logger.LogError($"DeductionService (CreateDeduction)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> UpdateDeduction(DeductionUpdateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }              
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.DeductionName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Deduction name is required";
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
                var repoPayload = new DeductionReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    DeductionName = payload.DeductionName,
                    DeductionId= payload.DeductionId,
                    IsModification = true,
                };
                string repoResponse = await _deductionsRepository.ProcessDeductions(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }
                long deductionId = Convert.ToInt64(repoResponse.Replace("Success", ""));
             
                foreach (var item in payload.EarningItems)
                {
                    var earningCoputationPayload = new DeductionComputationReq
                    {
                        CreatedByUserId = accessUser.data.UserId,
                        DateCreated = DateTime.Now,
                        DeductionId = deductionId,
                        EarningsItemId = item.EarningsItemId,
                        IsDelete = false
                    };
                    await _deductionsRepository.ProcessDeductionComputation(earningCoputationPayload);
                }
                var deductions = await _deductionsRepository.GetDeductionComputation(deductionId);
                foreach (var deductionComputation in deductions)
                {
                    if (payload.EarningItems.Any(p => p.EarningsItemId != deductionComputation.EarningsItemId))
                    {
                        var earningCoputationPayload = new DeductionComputationReq
                        {
                            CreatedByUserId = accessUser.data.UserId,
                            DateCreated = DateTime.Now,
                            DeductionId = deductionId,
                            EarningsItemId = deductionComputation.EarningsItemId,
                            IsDelete = true
                        };
                        await _deductionsRepository.ProcessDeductionComputation(earningCoputationPayload);
                    }
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "UpdateEarning",
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
                _logger.LogError($"DeductionService (UpdateEarning)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> DeleteDeduction(long DeductionId, string Comments, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
              
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(Comments))
                {
                    isModelStateValidate = false;
                    validationMessage += "Comments is required";
                }
                if (DeductionId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || DeductionId is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new DeductionDeleteReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    DeductionId = DeductionId,
                    DeleteComment = Comments
                };
                string repoResponse = await _deductionsRepository.DeleteDeduction(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }


                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "DeleteDeduction",
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
                _logger.LogError($"DeductionService (DeleteDeduction)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<IEnumerable<DeductionView>>> GetDeductions(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<IEnumerable<DeductionView>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var result = await _deductionsRepository.GetDeduction(accessUser.data.CompanyId);
                var earningsView = new List<DeductionView>();
                foreach (var item in result)
                {
                    var computations = await _deductionsRepository.GetDeductionComputation(item.DeductionId);
                    earningsView.Add(new DeductionView
                    {
                        CreatedByUserId = item.CreatedByUserId,
                        DateCreated = item.DateCreated,
                        DeductionId = item.DeductionId,
                        DeductionName = item.DeductionName,
                        DeductionComputation = computations,
                    });
                }
                return new ExecutedResult<IEnumerable<DeductionView>>() { responseMessage = ((int)ResponseCode.Ok).ToString().ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = earningsView };
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeductionService (GetDeductions)=====>{ex}");
                return new ExecutedResult<IEnumerable<DeductionView>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<DeductionView>> GetDeductionById(long Id, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<DeductionView>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var result = await _deductionsRepository.GetDeductionById(Id);

                var computations = await _deductionsRepository.GetDeductionComputation(result.DeductionId);
                var earningsView = new DeductionView
                {
                    CreatedByUserId = result.CreatedByUserId,
                    DateCreated = result.DateCreated,
                    DeductionId = result.DeductionId,
                    DeductionName = result.DeductionName,
                    DeductionComputation = computations,
                };
                return new ExecutedResult<DeductionView>() { responseMessage = ((int)ResponseCode.Ok).ToString().ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = earningsView };
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeductionService (GetDeductionById)=====>{ex}");
                return new ExecutedResult<DeductionView>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
    }
}
