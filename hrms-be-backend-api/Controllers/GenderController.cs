using hrms_be_backend_business.ILogic;
using hrms_be_backend_business.Logic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace hrms_be_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GenderController : BaseController
    {
        private readonly ILogger<GenderController> _logger;
        private readonly IGenderService _GenderService;

        public GenderController(ILogger<GenderController> logger, IGenderService GenderService)
        {
            _logger = logger;
            _GenderService = GenderService;
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
            return this.CustomResponse(await _GenderService.GetAllGender(accessToken, claim, RemoteIpAddress, RemotePort));
        }
    }
}
