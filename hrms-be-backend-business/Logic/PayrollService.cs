using hrms_be_backend_business.Helpers;
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
using MimeKit.Cryptography;
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
                var payrollEarnings = await _payrollRepository.GetPayrollEarnings(payrollId);
                foreach (var item in payrollEarnings)
                {
                    if (payload.Earnings.Any(p => p.EarningsItemId != item.EarningItemsId))
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
                    if (payload.Deductions.Any(p => p.DeductionId != item.DeductionId))
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
                    if (payload.Earnings.Any(p => p.EarningsItemId != item.EarningItemsId))
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
                    if (payload.Deductions.Any(p => p.DeductionId != item.DeductionId))
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
                            var deductionComputations = await _deductionsRepository.GetDeductionComputation(item.DeductionId);
                            foreach (var deductionItem in deductionComputations)
                            {
                                decimal deductionItemAmount = earnings.Where(p => p.EarningItemsId == deductionItem.EarningsItemId).Select(p => p.EarningItemAmount).FirstOrDefault();
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
                if (taxPayable.Count>0)
                {
                    taxPayable.OrderBy(p => p.StepNumber);
                    foreach (var item in taxPayable)
                    {
                        if (!item.IsLast)
                        {
                            if (item.PayableAmount >= taxIncomeRemained)
                            {
                                decimal percentage = decimal.Divide(item.PayablePercentage, 100);
                                decimal amt = decimal.Multiply(item.PayableAmount, percentage);
                                taxIncomeRemained -= item.PayableAmount;
                                taxPayableAmount += amt;
                            }
                        }
                        else
                        {
                            decimal percentage = decimal.Divide(item.PayablePercentage, 100);
                            decimal amt = decimal.Multiply(taxIncomeRemained, percentage);
                            taxPayableAmount += amt;
                            break;
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
                    return new ExecutedResult<IEnumerable<PayrollCyclesVm>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

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
    }
}
