using hrms_be_backend_business.Helpers;
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
using System.Text;

namespace hrms_be_backend_business.Logic
{
    public class PayrollService : IPayrollService
    {
        private readonly ILogger<PayrollService> _logger;
        private readonly IDeductionsRepository _deductionsRepository;
        private readonly IEarningsRepository _earningsRepository;
        private readonly IPayrollRepository _payrollRepository;
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;
        private readonly IUriService _uriService;
        private readonly JwtConfig _jwt;
        private readonly IAuditLog _audit;
        public PayrollService(IOptions<JwtConfig> jwt, ILogger<PayrollService> logger, IPayrollRepository payrollRepository, IEarningsRepository earningsRepository, IDeductionsRepository deductionsRepository, IAuditLog audit, IAuthService authService, IUriService uriService)
        {
            _logger = logger;
            _earningsRepository = earningsRepository;
            _deductionsRepository = deductionsRepository;
            _jwt = jwt.Value;
            _audit = audit;
            _authService = authService;
            _payrollRepository = payrollRepository;
            _uriService = uriService;
        }
        public async Task<ExecutedResult<string>> CreatePayroll(PayrollCreateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.PayrollTitle))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Title is required";
                }
                if (string.IsNullOrEmpty(payload.PayrollDescription))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Description is required";
                }
                if (payload.CurrencyId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Currency is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new PayrollReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    CurrencyId = payload.CurrencyId,
                    PayrollDescription = payload.PayrollDescription,
                    PayrollTitle = payload.PayrollTitle,
                    IsModification = false,
                };
                string repoResponse = await _payrollRepository.ProcessPayroll(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }
                long Id = Convert.ToInt64(repoResponse.Replace("Success", ""));


                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "CreatePayroll",
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
                _logger.LogError($"PayrollService (CreatePayroll)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> UpdatePayroll(PayrollUpdateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.PayrollTitle))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Title is required";
                }
                if (string.IsNullOrEmpty(payload.PayrollDescription))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Description is required";
                }
                if (payload.CurrencyId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Currency is required";
                }
                if (payload.PayrollId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Payroll Id is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new PayrollReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    CurrencyId = payload.CurrencyId,
                    PayrollDescription = payload.PayrollDescription,
                    PayrollTitle = payload.PayrollTitle,
                    IsModification = true,
                    PayrollId = payload.PayrollId,
                };
                string repoResponse = await _payrollRepository.ProcessPayroll(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }
                long Id = Convert.ToInt64(repoResponse.Replace("Success", ""));


                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "UpdatePayroll",
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
                _logger.LogError($"PayrollService (UpdatePayroll)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> DeletePayroll(long PayrollId, string Comments, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }

                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(Comments))
                {
                    isModelStateValidate = false;
                    validationMessage += "Comments is required";
                }
                if (PayrollId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || PayrollId is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new PayrollDeleteReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    PayrollId = PayrollId,
                    DeleteComment = Comments
                };
                string repoResponse = await _payrollRepository.DeletePayroll(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }


                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "DeletePayroll",
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
                _logger.LogError($"PayrollService (DeletePayroll)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }

        public async Task<PagedExcutedResult<IEnumerable<PayrollAllView>>> GetPayrolls(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            long totalRecords = 0;
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<PayrollAllView>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.AuthorizationError).ToString(), ResponseCode.AuthorizationError.ToString());

                }
                var result = await _payrollRepository.GetPayroll(accessUser.data.CompanyId, filter.PageNumber, filter.PageSize);
                if (result == null)
                {
                    return PaginationHelper.CreatePagedReponse<PayrollAllView>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }
                if (result.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<PayrollAllView>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }
                var returnData = new List<PayrollAllView>();
                foreach (var item in result.data)
                {
                    var deductions = await _payrollRepository.GetPayrollDeductions(item.PayrollId);
                    var decutionList = new List<string>();
                    foreach (var dec in deductions)
                    {
                        decutionList.Add(dec.DeductionName);
                    }
                    returnData.Add(new PayrollAllView
                    {
                        CreatedByUserName = item.CreatedByUserName,
                        CurrencyName = item.CurrencyName,
                        Payday = item.Payday,
                        PayrollCycleName = item.PayrollCycleName,
                        PayrollId = item.PayrollId,
                        PayRollTitle = item.PayRollTitle,
                        Deductions = decutionList,
                    });
                }

                totalRecords = result.totalRecords;

                var pagedReponse = PaginationHelper.CreatePagedReponse<PayrollAllView>(returnData, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Ok).ToString(), ResponseCode.Ok.ToString());

                return pagedReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollService (GetPayrolls)=====>{ex}");
                return PaginationHelper.CreatePagedReponse<PayrollAllView>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Exception).ToString(), $"Unable to process the transaction, kindly contact us support");
            }
        }
        public async Task<ExecutedResult<PayrollSingleView>> GetPayrollById(long Id, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<PayrollSingleView>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var result = await _payrollRepository.GetPayrollById(Id);
                if (result == null)
                {
                    return new ExecutedResult<PayrollSingleView>() { responseMessage = ((int)ResponseCode.NotFound).ToString().ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }
               
                var computations = await _earningsRepository.GetEarningsComputation(result.EarningsId);
                var cra = await _earningsRepository.GetEarningsCRA(accessUser.data.CompanyId);

                var deductions = await _deductionsRepository.GetDeduction(accessUser.data.CompanyId);
                StringBuilder restatedGross = new StringBuilder();
                restatedGross.Append("Gross ");
                foreach (var deduction in deductions)
                {
                    restatedGross.Append($" - {deduction.DeductionName}");
                }
                string craDetails = $"{cra.EarningsCRAPercentage}% * RestatedGross <br/><br/> OR <br/><br/>";
                craDetails = $"Higher of {cra.EarningsCRAHigherOfPercentage}% * RestatedGross or {cra.EarningsCRAHigherOfValue.ToString("0,0.00")}";

                var earningsView = new EarningsView
                {
                    CreatedByUserId = result.CreatedByUserId,
                    DateCreated = result.DateCreated,
                    EarningsId = result.EarningsId,
                    EarningsName = result.EarningsName,
                    EarningsComputations = computations,
                    EarningsCRA = craDetails,
                    RestatedGross = restatedGross.ToString()
                };
                return new ExecutedResult<EarningsView>() { responseMessage = ((int)ResponseCode.Ok).ToString().ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = earningsView };
            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollService (GetEarning)=====>{ex}");
                return new ExecutedResult<EarningsView>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }

        public async Task<ExecutedResult<IEnumerable<PayrollCyclesVm>>> GetPayrollCycles(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<IEnumerable<PayrollCyclesVm>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var returnData = await _payrollRepository.GetPayrollCycles();
                if (returnData == null)
                {
                    return new ExecutedResult<IEnumerable<PayrollCyclesVm>>() { responseMessage = ((int)ResponseCode.NotFound).ToString().ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }
                return new ExecutedResult<IEnumerable<PayrollCyclesVm>>() { responseMessage = ((int)ResponseCode.Ok).ToString().ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = returnData };
            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollService (GetPayrollCycles)=====>{ex}");
                return new ExecutedResult<IEnumerable<PayrollCyclesVm>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
    }
}
