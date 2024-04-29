using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace hrms_be_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : BaseController
    {
        private readonly IPayrollService _payrollService;
        public ReportController(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }
        [HttpGet("GetPayrollRunnedReport")]
        [ProducesResponseType(typeof(ExecutedResult<PayrollRunnedReportVm>), 200)]
        public async Task<IActionResult> GetPayrollRunnedReport([FromQuery] long PayrollRunnedId)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _payrollService.GetPayrollRunnedReport(PayrollRunnedId, accessToken, claim, RemoteIpAddress, RemotePort));
        }

        [HttpGet("DownloadPayrollRunnedReport")]
        public async Task<IActionResult> DownloadPayrollRunnedReport(long PayRollRunnedId)
        {
            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var returnData = await _payrollService.DownloadPayrollRunnedReport(PayRollRunnedId, accessToken, claim, RemoteIpAddress, RemotePort);

            string fileName = "doc.xlsx";

            return File(returnData.data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

        }
    }
}
