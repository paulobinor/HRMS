using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace hrms_be_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PayrollController : BaseController
    {
        private readonly IPayrollService _payrollService;
        public PayrollController(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }

        [HttpPost("CreatePayroll")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> CreatePayroll(PayrollCreateDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _payrollService.CreatePayroll(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }

        [HttpPost("UpdatePayroll")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> UpdatePayroll(PayrollUpdateDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _payrollService.UpdatePayroll(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("DeletePayroll")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> DeletePayroll(long PayrollId, string Comments)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _payrollService.DeletePayroll(PayrollId, Comments, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("RunPayroll")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> RunPayroll(RunPayrollDto Payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _payrollService.RunPayroll(Payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }


        [HttpGet("GetPayrolls")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<PayrollAllView>>), 200)]
        public async Task<IActionResult> GetPayrolls([FromQuery] PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _payrollService.GetPayrolls(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }

        [HttpGet("GetPayrollById")]
        [ProducesResponseType(typeof(ExecutedResult<PayrollSingleView>), 200)]
        public async Task<IActionResult> GetPayrollById([FromQuery] long PayrollId)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _payrollService.GetPayrollById(PayrollId, accessToken, claim, RemoteIpAddress, RemotePort));
        }

        [HttpGet("GetPayrollCycles")]
        [ProducesResponseType(typeof(ExecutedResult<IEnumerable<PayrollCyclesVm>>), 200)]
        public async Task<IActionResult> GetPayrollCycles()
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _payrollService.GetPayrollCycles(accessToken, claim, RemoteIpAddress, RemotePort));
        }


        [HttpGet("GetPayrollRunned")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<PayrollRunnedDetailsVm>>), 200)]
        public async Task<IActionResult> GetPayrollRunned([FromQuery] PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _payrollService.GetPayrollRunned(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetPayrollRunnedForReport")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<PayrollRunnedDetailsVm>>), 200)]
        public async Task<IActionResult> GetPayrollRunnedForReport([FromQuery] PaginationFilter filter, DateTime DateFrom, DateTime DateTo)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _payrollService.GetPayrollRunnedForReport(filter, DateFrom, DateTo, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }

        [HttpGet("GetPayrollRunnedDetails")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<PayrollRunnedDetailsVm>>), 200)]
        public async Task<IActionResult> GetPayrollRunnedDetails([FromQuery] PaginationFilter filter, long PayrollRunnedId)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _payrollService.GetPayrollRunnedDetails(filter, PayrollRunnedId, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetPayrollRunnedSummary")]
        [ProducesResponseType(typeof(ExecutedResult<PayrollRunnedSummaryVm>), 200)]
        public async Task<IActionResult> GetPayrollRunnedSummary([FromQuery] long PayrollRunnedId)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _payrollService.GetPayrollRunnedSummary(PayrollRunnedId, accessToken, claim, RemoteIpAddress, RemotePort));
        }
    }
}
