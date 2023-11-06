using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.Configuration;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;

namespace hrms_be_backend_business.Logic
{
    public class TaxService: ITaxService
    {
        private readonly ILogger<TaxService> _logger;
        private readonly IEarningsRepository _earningsRepository;
        private readonly IDeductionsRepository _deductionsRepository;
        private readonly ITaxRepository _taxRepository;
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;
        private readonly JwtConfig _jwt;
        private readonly IAuditLog _audit;
        public TaxService(IOptions<JwtConfig> jwt, ILogger<TaxService> logger, IEarningsRepository earningsRepository, IDeductionsRepository deductionsRepository, ITaxRepository taxRepository, IAuditLog audit, IAuthService authService)
        {
            _logger = logger;
            _earningsRepository = earningsRepository;
            _deductionsRepository = deductionsRepository;
            _taxRepository = taxRepository;
            _jwt = jwt.Value;
            _audit = audit;
            _authService = authService;
        }
        public async Task<ExecutedResult<TaxView>> GetTaxDetails(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<TaxView>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
              
                var taxPaybles = await _taxRepository.GetTaxPayable(accessUser.data.CompanyId);

                var deductions = await _deductionsRepository.GetDeduction(accessUser.data.CompanyId);
                StringBuilder taxableIncome = new StringBuilder();
                taxableIncome.Append("Gross ");              
                foreach (var deduction in deductions)
                {                   
                    taxableIncome.Append($" - {deduction.DeductionName}");
                }
                taxableIncome.Append($" - CRA");

                StringBuilder taxPayable = new StringBuilder();
                int count = 0;string operatorSign = "";
                foreach (var tax in taxPaybles)
                {
                    if (count > 0)
                    {
                        operatorSign = "<br/> + <br/>";
                    }
                    if (tax.IsLast)
                    {
                        taxPayable.Append($" Step {tax.StepNumber} - {tax.PayableAmount.ToString("0,00")} of taxable income * {tax.PayablePercentage}% <hr/>");
                    }
                    else
                    {
                        taxPayable.Append($" Step {tax.StepNumber} - over {tax.PayableAmount.ToString("0,00")} of taxable income * {tax.PayablePercentage}% <hr/>");
                    }
                  
                    count++;
                }
                var taxView = new TaxView
                {
                    TaxIncomeComputation = taxableIncome.ToString(),
                    TaxPayableComputation = taxPayable.ToString(),
                };
                return new ExecutedResult<TaxView>() { responseMessage = ((int)ResponseCode.Ok).ToString().ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = taxView };
            }
            catch (Exception ex)
            {
                _logger.LogError($"TaxService (GetTax)=====>{ex}");
                return new ExecutedResult<TaxView>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
    }
}
