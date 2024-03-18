using hrms_be_backend_business.ILogic;
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
    public class MaritalStatusController : BaseController
    {
        private readonly ILogger<MaritalStatusController> _logger;
        private readonly IMaritalStatusService _MaritalStatusService;

        public MaritalStatusController(ILogger<MaritalStatusController> logger, IMaritalStatusService MaritalStatusService)
        {
            _logger = logger;
            _MaritalStatusService = MaritalStatusService;
        }

        [HttpGet("GetAllGender")]
        [ProducesResponseType(typeof(ExecutedResult<IEnumerable<GenderDTO>>), 200)]
        public async Task<IActionResult> GetAllGender()
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _MaritalStatusService.GetAllMaritalStatus(accessToken, claim, RemoteIpAddress, RemotePort));
        }
    }
}
