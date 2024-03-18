using hrms_be_backend_api.ExitModuleController.Controller;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_business.Logic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.RepoPayload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace hrms_be_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExitClearanceSetupController : ControllerBase
    {
        private readonly ILogger<ExitClearanceSetupController> _logger;
        private readonly IExitClearanceSetupService _exitClearanceSetupService;
        public ExitClearanceSetupController(ILogger<ExitClearanceSetupController> logger, IExitClearanceSetupService exitClearanceSetupService)
        {
            _logger = logger;
            _exitClearanceSetupService = exitClearanceSetupService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateExitClearanceSetup(ExitClearanceSetupDTO request)
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _exitClearanceSetupService.CreateExitClearanceSetup(request, accessToken, RemoteIpAddress));

        }

        [HttpPost]
        [Route("UpdateExitClearanceSetup")]
        [Authorize]
        public async Task<IActionResult> UpdateExitClearanceSetup([FromBody] ExitClearanceSetupDTO updateDTO)
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _exitClearanceSetupService.UpdateExitClearanceSetup(updateDTO, accessToken, RemoteIpAddress));


        }

        //[Authorize]
        [HttpGet]
        [Route("GetExitClearanceSetupByID/{exitClearanceSetupID}")]
        [Authorize]
        public async Task<IActionResult> GetExitClearanceSetupByID(long exitClearanceSetupID)
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _exitClearanceSetupService.GetExitClearanceSetupByID(exitClearanceSetupID, accessToken, RemoteIpAddress));


        }

        //[Authorize]
        [HttpGet]
        [Route("GetExitClearanceSetupByCompanyID/{companyId}")]
        [Authorize]
        public async Task<IActionResult> GetExitClearanceSetupByCompanyID(long companyId, [FromQuery] PaginationFilter filter)
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _exitClearanceSetupService.GetExitClearanceSetupByCompanyID(filter, companyId, accessToken, RemoteIpAddress));


        }

        [Authorize]
        [HttpPost]
        [Route("DeleteExitClearanceSetup")]
        [Authorize]
        public async Task<IActionResult> DeleteExitClearanceSetup([FromBody] ExitClearanceSetupDTO request)
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _exitClearanceSetupService.DeleteExitClearanceSetup(request, accessToken, RemoteIpAddress));

        }
    }
}
