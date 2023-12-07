using hrms_be_backend_business.ILogic;
using hrms_be_backend_business.Logic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
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
    public class IdentificationTypeController : BaseController
    {

        private readonly ILogger<IdentificationTypeController> _logger;
        private readonly IIdentificationTypeService _identificationTypeService;

        public IdentificationTypeController(ILogger<IdentificationTypeController> logger, IIdentificationTypeService identificationTypeService)
        {
            _logger = logger;
            _identificationTypeService = identificationTypeService;
        }
        [HttpPost("CreateIdentificationType")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> CreateIdentificationType(CreateIdentificationTypeDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _identificationTypeService.CreateIdentificationType(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("UpdateIdentificationType")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> UpdateIdentificationType(UpdateIdentificationTypeDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _identificationTypeService.UpdateIdentificationType(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetIdenticationTypes")]
        [ProducesResponseType(typeof(ExecutedResult<IEnumerable<IdenticationTypeVm>>), 200)]
        public async Task<IActionResult> GetIdenticationTypes()
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _identificationTypeService.GetIdenticationTypes(accessToken, claim, RemoteIpAddress, RemotePort));
        }
    }
}
