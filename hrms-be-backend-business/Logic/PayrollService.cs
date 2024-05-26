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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Security.Claims;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace hrms_be_backend_business.Logic
{
    public class PayrollService : IPayrollService
    {
        private readonly ILogger<PayrollService> _logger;
        private readonly IDeductionsRepository _deductionsRepository;
        private readonly IEarningsRepository _earningsRepository;
        private readonly IPayrollRepository _payrollRepository;
        private readonly ITaxRepository _taxRepository;
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;
        private readonly IUriService _uriService;
        private readonly JwtConfig _jwt;
        private readonly IAuditLog _audit;
        public PayrollService(IOptions<JwtConfig> jwt, ILogger<PayrollService> logger, IPayrollRepository payrollRepository, IEarningsRepository earningsRepository, IDeductionsRepository deductionsRepository, ITaxRepository taxRepository, IAuditLog audit, IAuthService authService, IUriService uriService)
        {
            _logger = logger;
            _earningsRepository = earningsRepository;
            _deductionsRepository = deductionsRepository;
            _jwt = jwt.Value;
            _audit = audit;
            _authService = authService;
            _payrollRepository = payrollRepository;
            _taxRepository = taxRepository;
            _uriService = uriService;
        }
        public async Task<ExecutedResult<string>> CreatePayroll(PayrollCreateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
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
                if (payload.PayrollCycleId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Payrol cycle is required";
                }
                if (payload.Earnings == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Payrol earning is required";
                }
                if (payload.Earnings.Any(p => p.EarningsItemId < 1))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || One of the earning contain invalid data, EarningsItemId must be greater than 0";
                }
                if (payload.Earnings.Any(p => p.EarningsItemAmount < 1))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || One of the earning contain invalid data, EarningsItemAmount must be greater than 0";
                }
                if (payload.Deductions == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Payrol deduction is required";
                }
                if (payload.Deductions.Any(p => p.DeductionId < 1))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || One of the deduction contain invalid data, deductionId must be greater than 0";
                }
                var checkDeductionFixed = payload.Deductions.Where(p => p.IsFixed && p.DeductionFixedAmount < 1).ToList();
                if (checkDeductionFixed.Count() > 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || One of the deduction contain invalid data, DeductionFixedAmount must be greater than 0";
                }
                var checkDeductionPercentage = payload.Deductions.Where(p => p.IsPercentage && p.DeductionPercentageAmount < 1).ToList();
                if (checkDeductionPercentage.Count() > 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || One of the deduction contain invalid data, DeductionPercentageAmount must be greater than 0";
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
                    Payday = payload.Payday,
                    PaydayCustomDayOfTheCycle = payload.PaydayCustomDayOfTheCycle,
                    PaydayLastDayOfTheCycle = payload.PaydayLastDayOfTheCycle,
                    PaydayLastWeekOfTheCycle = payload.PaydayLastWeekOfTheCycle,
                    PayrollCycleId = payload.PayrollCycleId,
                    ProrationPolicy = payload.ProrationPolicy,
                    IsModification = false,
                };
                string repoResponse = await _payrollRepository.ProcessPayroll(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }
                long payrollId = Convert.ToInt64(repoResponse.Replace("Success", ""));
                foreach (var item in payload.Earnings)
                {
                    var payrollEarningsReq = new PayrollEarningsReq
                    {
                        EarningsItemAmount = item.EarningsItemAmount,
                        CreatedByUserId = accessUser.data.UserId,
                        DateCreated = DateTime.Now,
                        EarningsItemId = item.EarningsItemId,
                        PayrollId = payrollId
                    };
                    await _payrollRepository.ProcessPayrollEarnings(payrollEarningsReq);
                }
                foreach (var item in payload.Deductions)
                {
                    var payrollDeductionReq = new PayrollDeductionReq
                    {
                        DeductionFixedAmount = item.DeductionFixedAmount,
                        CreatedByUserId = accessUser.data.UserId,
                        DateCreated = DateTime.Now,
                        DeductionPercentageAmount = item.DeductionPercentageAmount,
                        DeductionId = item.DeductionId,
                        IsFixed = item.IsFixed,
                        IsPercentage = item.IsPercentage,
                        PayrollId = payrollId
                    };
                    await _payrollRepository.ProcessPayrollDeduction(payrollDeductionReq);
                }

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
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";
                if (payload.PayrollId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  PayrollId is required";
                }
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
                if (payload.PayrollCycleId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Payrol cycle is required";
                }
                if (payload.Earnings == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Payrol earning is required";
                }
                if (payload.Earnings.Any(p => p.EarningsItemId < 1))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || One of the earning contain invalid data, EarningsItemId must be greater than 0";
                }
                if (payload.Earnings.Any(p => p.EarningsItemAmount < 1))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || One of the earning contain invalid data, EarningsItemAmount must be greater than 0";
                }
                if (payload.Deductions == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Payrol deduction is required";
                }
                if (payload.Deductions.Any(p => p.DeductionId < 1))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || One of the deduction contain invalid data, deductionId must be greater than 0";
                }
                if (payload.Deductions.Any(p => p.IsFixed) && payload.Deductions.Any(p => p.DeductionFixedAmount < 1))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || One of the deduction contain invalid data, DeductionFixedAmount must be greater than 0";
                }
                if (payload.Deductions.Any(p => p.IsPercentage) && payload.Deductions.Any(p => p.DeductionPercentageAmount < 1))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || One of the deduction contain invalid data, DeductionPercentageAmount must be greater than 0";
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
                    Payday = payload.Payday,
                    PaydayCustomDayOfTheCycle = payload.PaydayCustomDayOfTheCycle,
                    PaydayLastDayOfTheCycle = payload.PaydayLastDayOfTheCycle,
                    PaydayLastWeekOfTheCycle = payload.PaydayLastWeekOfTheCycle,
                    PayrollCycleId = payload.PayrollCycleId,
                    ProrationPolicy = payload.ProrationPolicy,
                    IsModification = true,
                    PayrollId = payload.PayrollId,
                };
                string repoResponse = await _payrollRepository.ProcessPayroll(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }
                long payrollId = Convert.ToInt64(repoResponse.Replace("Success", ""));
                foreach (var item in payload.Earnings)
                {
                    var payrollEarningsReq = new PayrollEarningsReq
                    {
                        EarningsItemAmount = item.EarningsItemAmount,
                        CreatedByUserId = accessUser.data.UserId,
                        DateCreated = DateTime.Now,
                        EarningsItemId = item.EarningsItemId,
                        PayrollId = payrollId
                    };
                    await _payrollRepository.ProcessPayrollEarnings(payrollEarningsReq);
                }
                var payrollEarnings = await _payrollRepository.GetPayrollEarnings(payrollId);
                foreach (var item in payrollEarnings)
                {
                    var checkEaningItem = payload.Earnings.Where(p => p.EarningsItemId == item.EarningItemsId).ToList();
                    if (checkEaningItem.Count() < 1)
                    {
                        var payrollEarningsDeleteReq = new PayrollEarningsDeleteReq
                        {
                            CreatedByUserId = accessUser.data.UserId,
                            DateCreated = DateTime.Now,
                            EarningsItemId = item.EarningsId,
                            PayrollId = payrollId
                        };
                        await _payrollRepository.DeletePayrollEarnings(payrollEarningsDeleteReq);
                    }
                }
                foreach (var item in payload.Deductions)
                {
                    var payrollDeductionReq = new PayrollDeductionReq
                    {
                        DeductionFixedAmount = item.DeductionFixedAmount,
                        CreatedByUserId = accessUser.data.UserId,
                        DateCreated = DateTime.Now,
                        DeductionPercentageAmount = item.DeductionPercentageAmount,
                        DeductionId = item.DeductionId,
                        IsFixed = item.IsFixed,
                        IsPercentage = item.IsPercentage,
                        PayrollId = payrollId
                    };
                    await _payrollRepository.ProcessPayrollDeduction(payrollDeductionReq);
                }
                var payrollDeductions = await _payrollRepository.GetPayrollDeductions(payrollId);
                foreach (var item in payrollDeductions)
                {
                    var checkDeduction = payload.Deductions.Where(p => p.DeductionId == item.DeductionId).ToList();
                    if (checkDeduction.Count() < 1)
                    {
                        var payrollDeductionDeleteReq = new PayrollDeductionDeleteReq
                        {
                            CreatedByUserId = accessUser.data.UserId,
                            DateCreated = DateTime.Now,
                            DeductionId = item.DeductionId,
                            PayrollId = payrollId
                        };
                        await _payrollRepository.DeletePayrollDeduction(payrollDeductionDeleteReq);
                    }
                }
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

        public async Task<ExecutedResult<string>> DeletePayroll(long PayrollId, string Comments, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
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

        public async Task<ExecutedResult<string>> RunPayroll(RunPayrollDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
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
                if (payload.PayrollId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  PayrollId is required";
                }
                if (string.IsNullOrEmpty(payload.Title))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Title is required";
                }
                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }

                var earnings = await _payrollRepository.GetPayrollEarnings(payload.PayrollId);
                decimal totalEarningsAmount = 0, taxIncome = 0;
                if (earnings.Count > 0)
                {
                    foreach (var item in earnings)
                    {
                        totalEarningsAmount += item.EarningItemAmount;
                    }
                }

                var deductions = await _payrollRepository.GetPayrollDeductions(payload.PayrollId);
                decimal deductionTotalAmount = 0;
                if (deductions.Count > 0)
                {
                    var payrollDeduction = new List<PayrollDeduction>();
                    foreach (var item in deductions)
                    {
                        decimal deductionAmount = 0;
                        if (item.IsFixed)
                        {
                            deductionTotalAmount += item.DeductionFixedAmount;
                            deductionAmount = item.DeductionFixedAmount;
                        }
                        if (item.IsPercentage)
                        {
                            var deduction = await _payrollRepository.GetPayrollDeductionComputation(item.DeductionId, payload.PayrollId);
                            if (deduction != null)
                            {
                                foreach (var deductionItem in deduction)
                                {
                                    decimal deductionItemAmount = deductionItem.EarningItemAmount;
                                    if (deductionItemAmount > 0)
                                    {
                                        if (item.DeductionPercentageAmount > 0)
                                        {
                                            decimal percentage = decimal.Divide(item.DeductionPercentageAmount, 100);
                                            decimal amt = decimal.Multiply(percentage, deductionItemAmount);
                                            deductionTotalAmount += amt;
                                            deductionAmount = amt;
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
                decimal restatedAmount = totalEarningsAmount - deductionTotalAmount;

                //----CRA Calculation
                #region CRA Calculation
                var cra = await _earningsRepository.GetEarningsCRA(accessUser.data.CompanyId);
                decimal craAmount = 0;
                if (cra != null)
                {
                    if (cra.CreatedByUserId > 0)
                    {
                        decimal percentage = decimal.Divide(Convert.ToDecimal(cra.EarningsCRAPercentage), 100);
                        craAmount = decimal.Multiply(percentage, restatedAmount);
                        decimal earningsCRAHigherOfPercentage = decimal.Divide(Convert.ToDecimal(cra.EarningsCRAHigherOfPercentage), 100);
                        decimal earningsCRAHigherOfPercentageAmount = decimal.Multiply(earningsCRAHigherOfPercentage, restatedAmount);
                        decimal amountToAdd = 0;
                        if (earningsCRAHigherOfPercentageAmount > cra.EarningsCRAHigherOfValue)
                        {
                            amountToAdd = earningsCRAHigherOfPercentageAmount;
                        }
                        else
                        {
                            amountToAdd = cra.EarningsCRAHigherOfValue;
                        }
                        craAmount += amountToAdd;
                    }
                }
                #endregion

                #region Tax Calculation              
                taxIncome = totalEarningsAmount - deductionTotalAmount - craAmount;

                var taxPayable = await _taxRepository.GetTaxPayable(accessUser.data.CompanyId);
                decimal taxPayableAmount = 0, taxIncomeRemained = taxIncome;
                if (taxPayable.Count > 0)
                {
                    taxPayable.OrderBy(p => p.StepNumber);
                    foreach (var item in taxPayable)
                    {
                        if (taxIncomeRemained > 0)
                        {
                            decimal percentage = decimal.Divide(item.PayablePercentage, 100);
                            if (item.IsLast)
                            {
                                decimal amt = decimal.Multiply(taxIncomeRemained, percentage);
                                taxIncomeRemained -= item.PayableAmount;
                                taxPayableAmount += amt;
                            }
                            else
                            {
                                decimal amt = decimal.Multiply(item.PayableAmount, percentage);
                                taxIncomeRemained -= item.PayableAmount;
                                taxPayableAmount += amt;
                            }


                        }
                    }
                }

                #endregion
                decimal NetDeduction = deductionTotalAmount + taxPayableAmount;
                decimal netincome = totalEarningsAmount - NetDeduction;
                var repoPayload = new RunPayrollReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    Title = payload.Title,
                    NetCRAAmount = craAmount,
                    NetRestatedAmount = restatedAmount,
                    NetTAXAmount = taxPayableAmount,
                    LoanRepayment = 0,
                    NetDeduction = deductionTotalAmount,
                    NetPay = decimal.Divide(netincome, 12),
                    TotalEarning = totalEarningsAmount,
                    PayrollId = payload.PayrollId,
                };


                string repoResponse = await _payrollRepository.RunPayroll(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "RunPayroll",
                    payload = JsonConvert.SerializeObject(payload),
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<string>() { responseMessage = "Run Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };


            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollService (RunPayroll)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }

        public async Task<PagedExcutedResult<IEnumerable<PayrollRunnedVm>>> GetPayrollRunned(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            long totalRecords = 0;
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<PayrollRunnedVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.AuthorizationError).ToString(), ResponseCode.AuthorizationError.ToString());

                }
                var result = await _payrollRepository.GetPayrollRunned(accessUser.data.UserId, filter.PageNumber, filter.PageSize);
                if (result == null)
                {
                    return PaginationHelper.CreatePagedReponse<PayrollRunnedVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }
                if (result.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<PayrollRunnedVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }

                totalRecords = result.totalRecords;

                var pagedReponse = PaginationHelper.CreatePagedReponse<PayrollRunnedVm>(result.data, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Ok).ToString(), ResponseCode.Ok.ToString());

                return pagedReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollService (GetPayrollRunned)=====>{ex}");
                return PaginationHelper.CreatePagedReponse<PayrollRunnedVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Exception).ToString(), $"Unable to process the transaction, kindly contact us support");
            }
        }
        public async Task<PagedExcutedResult<IEnumerable<PayrollRunnedVm>>> GetPayrollRunnedForReport(PaginationFilter filter, string DateFrom, string DateTo, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            long totalRecords = 0;
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<PayrollRunnedVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.AuthorizationError).ToString(), ResponseCode.AuthorizationError.ToString());

                }
                var result = await _payrollRepository.GetPayrollRunnedForReport(accessUser.data.UserId, filter.SearchValue, filter.PageNumber, filter.PageSize, DateFrom, DateTo);
                if (result == null)
                {
                    return PaginationHelper.CreatePagedReponse<PayrollRunnedVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.NotFound.ToString());
                }
                if (result.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<PayrollRunnedVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.NotFound.ToString());
                }

                totalRecords = result.totalRecords;

                var pagedReponse = PaginationHelper.CreatePagedReponse<PayrollRunnedVm>(result.data, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Ok).ToString(), ResponseCode.Ok.ToString());

                return pagedReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollService (GetPayrollRunned)=====>{ex}");
                return PaginationHelper.CreatePagedReponse<PayrollRunnedVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Exception).ToString(), $"Unable to process the transaction, kindly contact us support");
            }
        }
        public async Task<PagedExcutedResult<IEnumerable<PayrollRunnedDetailsVm>>> GetPayrollRunnedDetails(PaginationFilter filter, long PayrollRunnedId, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            long totalRecords = 0;
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<PayrollRunnedDetailsVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.AuthorizationError).ToString(), ResponseCode.AuthorizationError.ToString());

                }
                var result = await _payrollRepository.GetPayrollRunnedDetails(PayrollRunnedId, filter.PageNumber, filter.PageSize);
                if (result == null)
                {
                    return PaginationHelper.CreatePagedReponse<PayrollRunnedDetailsVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }
                if (result.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<PayrollRunnedDetailsVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }

                totalRecords = result.totalRecords;

                var pagedReponse = PaginationHelper.CreatePagedReponse<PayrollRunnedDetailsVm>(result.data, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Ok).ToString(), ResponseCode.Ok.ToString());

                return pagedReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollService (GetPayrollRunnedDetails)=====>{ex}");
                return PaginationHelper.CreatePagedReponse<PayrollRunnedDetailsVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Exception).ToString(), $"Unable to process the transaction, kindly contact us support");
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
                var result = await _payrollRepository.GetPayroll(accessUser.data.CompanyId, filter.PageNumber, filter.PageSize, filter.SearchValue);
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
                    return new ExecutedResult<PayrollSingleView>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var payroll = await _payrollRepository.GetPayrollById(Id);
                if (payroll == null)
                {
                    return new ExecutedResult<PayrollSingleView>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }
                if (payroll.PayrollId < 1)
                {
                    return new ExecutedResult<PayrollSingleView>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }
                var returnData = new PayrollSingleView();
                returnData.PayrollId = payroll.PayrollId;
                returnData.Payday = payroll.Payday;
                returnData.PayRollTitle = payroll.PayRollTitle;
                returnData.PayRollDescription = payroll.PayRollDescription;
                returnData.ProrationPolicy = payroll.ProrationPolicy;
                returnData.CurrencyId = payroll.CurrencyId;
                returnData.CurrencyName = payroll.CurrencyName;
                returnData.CurrencyCode = payroll.CurrencyCode;
                returnData.PayrollCycleId = payroll.PayrollCycleId;
                returnData.PayrollCycleName = payroll.PayrollCycleName;
                returnData.PaydayCustomDayOfTheCycle = payroll.PaydayCustomDayOfTheCycle;
                returnData.PaydayLastWeekOfTheCycle = payroll.PaydayLastWeekOfTheCycle;
                returnData.PaydayLastDayOfTheCycle = payroll.PaydayLastDayOfTheCycle;

                var earnings = await _payrollRepository.GetPayrollEarnings(Id);
                decimal totalEarningsAmount = 0, taxIncome = 0; string restatedGrossComputation = "", taxComputation = "", earningName = "";
                if (earnings.Count > 0)
                {

                    earningName = earnings.FirstOrDefault().EarningsName;
                    returnData.EarningsName = earningName;
                    restatedGrossComputation = earningName;
                    taxComputation = earningName;
                    var payrollEarnings = new List<PayrollEarnings>();
                    foreach (var item in earnings)
                    {
                        totalEarningsAmount += item.EarningItemAmount;
                        payrollEarnings.Add(new PayrollEarnings()
                        {
                            EarningsItemAmount = item.EarningItemAmount,
                            EarningsItemId = item.EarningItemsId,
                            EarningsItemName = item.EarningItemName
                        });
                    }
                    returnData.EarningsItems = payrollEarnings;
                }
                returnData.TotalEarningAmount = totalEarningsAmount;

                var payrollPayments = new List<PayrollPayments>();
                payrollPayments.Add(new PayrollPayments
                {
                    PaymentAmount = decimal.Divide(totalEarningsAmount, 12),
                    PaymentName = earningName,
                    PaymentSubTitle = $"{earningName} / 12"
                });
                var deductions = await _payrollRepository.GetPayrollDeductions(Id);
                decimal deductionTotalAmount = 0;
                if (deductions.Count > 0)
                {
                    var payrollDeduction = new List<PayrollDeduction>();
                    foreach (var item in deductions)
                    {
                        restatedGrossComputation += $" - {item.DeductionName}";
                        taxComputation += $" - {item.DeductionName}";
                        decimal deductionAmount = 0;
                        if (item.IsFixed)
                        {
                            deductionTotalAmount += item.DeductionFixedAmount;
                            deductionAmount = item.DeductionFixedAmount;
                        }
                        if (item.IsPercentage)
                        {
                            var deduction = await _payrollRepository.GetPayrollDeductionComputation(item.DeductionId, Id);
                            if (deduction != null)
                            {
                                foreach (var deductionItem in deduction)
                                {
                                    decimal deductionItemAmount = deductionItem.EarningItemAmount;
                                    if (deductionItemAmount > 0)
                                    {
                                        if (item.DeductionPercentageAmount > 0)
                                        {
                                            decimal percentage = decimal.Divide(item.DeductionPercentageAmount, 100);
                                            decimal amt = decimal.Multiply(percentage, deductionItemAmount);
                                            deductionTotalAmount += amt;
                                            deductionAmount = amt;
                                        }
                                    }
                                }
                            }
                        }

                        payrollPayments.Add(new PayrollPayments
                        {
                            PaymentAmount = decimal.Divide(deductionAmount, 12),
                            PaymentName = $"{item.DeductionName}",
                            PaymentSubTitle = $"{item.DeductionName} / 12"
                        });
                        payrollDeduction.Add(new PayrollDeduction()
                        {
                            DeductionFixedAmount = item.DeductionFixedAmount,
                            DeductionPercentageAmount = item.DeductionPercentageAmount,
                            DeductionId = item.DeductionId,
                            DeductionName = item.DeductionName,
                            IsFixed = item.IsFixed,
                            IsPercentage = item.IsPercentage,
                        });
                    }
                    returnData.PayrollDeductions = payrollDeduction;
                }
                returnData.DeductionTotalAmount = deductionTotalAmount;
                decimal restatedAmount = totalEarningsAmount - deductionTotalAmount;
                returnData.RestatedGrossAmount = restatedAmount;

                //----CRA Calculation
                #region CRA Calculation
                var cra = await _earningsRepository.GetEarningsCRA(accessUser.data.CompanyId);
                decimal craAmount = 0;
                if (cra != null)
                {
                    if (cra.CreatedByUserId > 0)
                    {
                        decimal percentage = decimal.Divide(Convert.ToDecimal(cra.EarningsCRAPercentage), 100);
                        craAmount = decimal.Multiply(percentage, restatedAmount);
                        decimal earningsCRAHigherOfPercentage = decimal.Divide(Convert.ToDecimal(cra.EarningsCRAHigherOfPercentage), 100);
                        decimal earningsCRAHigherOfPercentageAmount = decimal.Multiply(earningsCRAHigherOfPercentage, restatedAmount);
                        decimal amountToAdd = 0;
                        if (earningsCRAHigherOfPercentageAmount > cra.EarningsCRAHigherOfValue)
                        {
                            amountToAdd = earningsCRAHigherOfPercentageAmount;
                        }
                        else
                        {
                            amountToAdd = cra.EarningsCRAHigherOfValue;
                        }
                        craAmount += amountToAdd;
                        returnData.CRAComputation = $"{cra.EarningsCRAPercentage}% of Restated Gross + (Higher of {cra.EarningsCRAHigherOfPercentage}% * Restated Gross or {cra.EarningsCRAHigherOfValue})";
                        returnData.CRAAmount = craAmount;
                    }
                }
                #endregion

                #region Tax Calculation
                taxComputation += " - CRA";
                taxIncome = totalEarningsAmount - deductionTotalAmount - craAmount;
                returnData.TaxIncomeAmount = taxIncome;

                var taxPayable = await _taxRepository.GetTaxPayable(accessUser.data.CompanyId);
                decimal taxPayableAmount = 0, taxIncomeRemained = taxIncome;
                if (taxPayable.Count > 0)
                {
                    taxPayable.OrderBy(p => p.StepNumber);
                    foreach (var item in taxPayable)
                    {
                        if (taxIncomeRemained > 0)
                        {
                            decimal percentage = decimal.Divide(item.PayablePercentage, 100);
                            if (item.IsLast)
                            {
                                decimal amt = decimal.Multiply(taxIncomeRemained, percentage);
                                taxIncomeRemained -= item.PayableAmount;
                                taxPayableAmount += amt;
                            }
                            else
                            {
                                decimal amt = decimal.Multiply(item.PayableAmount, percentage);
                                taxIncomeRemained -= item.PayableAmount;
                                taxPayableAmount += amt;
                            }


                        }
                    }
                }
                returnData.TaxPayableAmount = taxPayableAmount;
                payrollPayments.Add(new PayrollPayments
                {
                    PaymentAmount = decimal.Divide(taxPayableAmount, 12),
                    PaymentName = "Tax",
                    PaymentSubTitle = $"Tax / 12"
                });
                decimal monthlyEarnings = decimal.Divide(totalEarningsAmount, 12);
                decimal monthlyDeduction = decimal.Divide(deductionTotalAmount, 12);
                payrollPayments.Add(new PayrollPayments
                {
                    PaymentAmount = monthlyEarnings - monthlyDeduction,
                    PaymentName = "Net Pay",
                    PaymentSubTitle = $"Net Pay / 12"
                });
                #endregion




                return new ExecutedResult<PayrollSingleView>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = returnData };
            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollService (GetPayrollById)=====>{ex}");
                return new ExecutedResult<PayrollSingleView>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }

        public async Task<ExecutedResult<IEnumerable<PayrollCyclesVm>>> GetPayrollCycles(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<IEnumerable<PayrollCyclesVm>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var returnData = await _payrollRepository.GetPayrollCycles();
                if (returnData == null)
                {
                    return new ExecutedResult<IEnumerable<PayrollCyclesVm>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }
                return new ExecutedResult<IEnumerable<PayrollCyclesVm>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = returnData };
            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollService (GetPayrollCycles)=====>{ex}");
                return new ExecutedResult<IEnumerable<PayrollCyclesVm>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<PayrollRunnedSummaryVm>> GetPayrollRunnedSummary(long PayrollRunnedId, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<PayrollRunnedSummaryVm>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }

                var repoResponse = await _payrollRepository.GetPayrollRunnedSummary(PayrollRunnedId);
                if (repoResponse == null)
                {
                    return new ExecutedResult<PayrollRunnedSummaryVm>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }

                return new ExecutedResult<PayrollRunnedSummaryVm>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = repoResponse };


            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollService (GetPayrollRunnedSummary)=====>{ex}");
                return new ExecutedResult<PayrollRunnedSummaryVm>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }

        public async Task<ExecutedResult<IEnumerable<PayrollRunnedReportVm>>> GetPayrollRunnedReport(long PayRollRunnedId, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<IEnumerable<PayrollRunnedReportVm>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var returnData = await _payrollRepository.GetPayrollRunnedReport(PayRollRunnedId);
                if (returnData == null)
                {
                    return new ExecutedResult<IEnumerable<PayrollRunnedReportVm>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }
                return new ExecutedResult<IEnumerable<PayrollRunnedReportVm>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = returnData };
            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollService (GetPayrollRunnedReport)=====>{ex}");
                return new ExecutedResult<IEnumerable<PayrollRunnedReportVm>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<byte[]>> DownloadPayrollRunnedReport(long PayRollRunnedId, string AccessKey, IEnumerable<Claim> claim, string AccessFromIPAddress, string AccessFromPort)
        {
            try
            {
                byte[] fileContents;

                var response = new ExecutedResult<byte[]>();
                var accessUser = await _authService.CheckUserAccess(AccessKey, AccessFromIPAddress);
                if (accessUser.data == null)
                {
                    response.responseCode = ((int)ResponseCode.AuthorizationError).ToString();
                    response.responseMessage = "Unauthorized User";
                    return response;
                }
                var payollRunned = await _payrollRepository.GetPayrollRunnedById(PayRollRunnedId);
                if (payollRunned.CompanyId != accessUser.data.CompanyId)
                {
                    response.responseCode = ((int)ResponseCode.AuthorizationError).ToString();
                    response.responseMessage = "Unauthorized User";
                    return response;
                }

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add($"PayrollReport_{payollRunned.Title.Replace(" ", "_")}");
                    worksheet.Cells[1, 1].Value = "S/N";
                    worksheet.Cells[1, 2].Value = "STAFF ID";
                    worksheet.Cells[1, 3].Value = "NAME";
                    worksheet.Cells[1, 4].Value = "GRADE";
                    worksheet.Cells[1, 5].Value = "DATE OF EMPLOYMENT";
                    worksheet.Cells[1, 6].Value = "DEPARTMENT";
                    worksheet.Cells[1, 7].Value = "TAX RATE";
                    worksheet.Cells[1, 8].Value = "LOCATION";
                    worksheet.Cells[1, 9].Value = "REMARK";

                    int colNumber = 10, earningColumnBegin = 10;
                    var payrollEarnings = await _payrollRepository.GetPayrollEarnings(payollRunned.PayrollId);
                    var payrollEarningForReportVm = new List<PayrollEarningForReportVm>();
                    foreach (var payrollEarning in payrollEarnings)
                    {
                        payrollEarningForReportVm.Add(new PayrollEarningForReportVm
                        {
                            Amount = payrollEarning.EarningItemAmount,
                            EarningItemId = payrollEarning.EarningItemsId,
                            EarningItemName = payrollEarning.EarningItemName,
                        });
                        worksheet.Cells[1, colNumber].Value = payrollEarning.EarningItemName.ToUpper();
                        colNumber++;
                    }
                    worksheet.Cells[1, colNumber].Value = "GROSS SALARY";
                    colNumber++;
                    worksheet.Cells[1, colNumber].Value = "TAX";
                    colNumber++;
                    var payrollDeductions = await _payrollRepository.GetPayrollDeductions(payollRunned.PayrollId);
                    var payrollDeductionForReportVm = new List<PayrollDeductionForReportVm>();
                    int deductionColumnBegin = colNumber;
                    foreach (var payrollDeduction in payrollDeductions)
                    {
                        decimal amount = 0;
                        if (payrollDeduction.IsFixed)
                        {
                            amount = payrollDeduction.DeductionFixedAmount;
                        }
                        if (payrollDeduction.IsPercentage)
                        {
                            var payrollDeductionComputation = await _payrollRepository.GetPayrollDeductionComputation(payrollDeduction.DeductionId, payollRunned.PayrollId);
                            decimal payrollDeductionComputationAmount = payrollDeductionComputation.Sum(p => p.EarningItemAmount);
                            if (payrollDeductionComputationAmount > 0)
                            {
                                amount = (payrollDeduction.DeductionPercentageAmount / 100) * payrollDeductionComputationAmount;
                            }
                        }
                        payrollDeductionForReportVm.Add(new PayrollDeductionForReportVm
                        {
                            Amount = amount,
                            DeductionId = payrollDeduction.DeductionId,
                            DeductionName = payrollDeduction.DeductionName,
                        });
                        worksheet.Cells[1, colNumber].Value = payrollDeduction.DeductionName.ToUpper();
                        colNumber++;
                    }
                    worksheet.Cells[1, colNumber].Value = "TOTAL DEDUCTION";
                    colNumber++;
                    worksheet.Cells[1, colNumber].Value = "NET PAY";
                    colNumber++;
                    var payrollRunnedReport = await _payrollRepository.GetPayrollRunnedReport(PayRollRunnedId);
                    int rowNumber = 2; int sno = 0;
                    foreach (var payrollRunned in payrollRunnedReport)
                    {
                        sno++;
                        worksheet.Cells[rowNumber, 1].Value = $"{sno}";
                        worksheet.Cells[rowNumber, 2].Value = $"{payrollRunned.StaffID}";
                        worksheet.Cells[rowNumber, 3].Value = $"{payrollRunned.EmployeeName}";
                        worksheet.Cells[rowNumber, 4].Value = $"{payrollRunned.GradeName}";
                        worksheet.Cells[rowNumber, 5].Value = $"{payrollRunned.ResumptionDate.ToString("dd-MMM-yyyy")}";
                        worksheet.Cells[rowNumber, 6].Value = $"{payrollRunned.DepartmentName}";
                        worksheet.Cells[rowNumber, 7].Value = $"TaxRate";
                        worksheet.Cells[rowNumber, 8].Value = $"{payrollRunned.BranchName}";
                        worksheet.Cells[rowNumber, 9].Value = $"{payrollRunned.EmploymentStatusName}";
                        int col = 10;
                        foreach (var payrollEarning in payrollEarningForReportVm)
                        {
                            worksheet.Cells[rowNumber, col].Value = $"{payrollEarning.Amount}";
                            col++;
                        }
                        worksheet.Cells[rowNumber, col].Value = $"{payrollRunned.GrossPay}";
                        col++;
                        worksheet.Cells[rowNumber, col].Value = $"{payrollRunned.TAX}";
                        col++;
                        foreach (var payrollDeduction in payrollDeductionForReportVm)
                        {
                            worksheet.Cells[rowNumber, col].Value = $"{payrollDeduction.Amount}";
                            col++;
                        }
                        worksheet.Cells[rowNumber, col].Value = $"{payrollRunned.TotalDeduction}";
                        col++;
                        worksheet.Cells[rowNumber, col].Value = $"{payrollRunned.NetPay}";
                        col++;
                        rowNumber++;
                    }
                    //Header Border
                    for (int k = 1; k <= colNumber; k++)
                    {
                        worksheet.Cells[1, k].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        worksheet.Cells[1, k].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[1, k].Style.Border.Top.Color.SetColor(Color.Black);
                        worksheet.Cells[1, k].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[1, k].Style.Border.Bottom.Color.SetColor(Color.Black);
                        worksheet.Cells[1, k].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[1, k].Style.Border.Left.Color.SetColor(Color.Black);
                        worksheet.Cells[1, k].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[1, k].Style.Border.Right.Color.SetColor(Color.Black);
                    }

                    //Row Border
                    for (int i = 1; i <= rowNumber; i++)
                    {
                        for (int k = 1; k <= colNumber; k++)
                        {
                            worksheet.Cells[i + 1, k].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            worksheet.Cells[i + 1, k].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[i + 1, k].Style.Border.Top.Color.SetColor(Color.Black);
                            worksheet.Cells[i + 1, k].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[i + 1, k].Style.Border.Bottom.Color.SetColor(Color.Black);
                            worksheet.Cells[i + 1, k].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[i + 1, k].Style.Border.Left.Color.SetColor(Color.Black);
                            worksheet.Cells[i + 1, k].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[i + 1, k].Style.Border.Right.Color.SetColor(Color.Black);
                        }
                    }
                    //Format
                    for (int i = 1; i <= colNumber; i++)
                    {
                        //worksheet.Column(i).Width = 20;
                        if (i > 10)
                        {
                            worksheet.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                    }
                    worksheet.View.ShowGridLines = false;

                    fileContents = package.GetAsByteArray();
                }

                response.responseCode = ((int)ResponseCode.Ok).ToString();
                response.responseMessage = "Success";
                response.data = fileContents;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"PayrollService (DownloadPayrollRunnedReport)=====>{ex}");
                return new ExecutedResult<byte[]>()
                {
                    data = null,
                    responseCode = ((int)ResponseCode.Exception).ToString(),
                    responseMessage = EnumHelper.GetEnumDescription(ResponseCode.Exception)
                };
            }
        }
    }
}
