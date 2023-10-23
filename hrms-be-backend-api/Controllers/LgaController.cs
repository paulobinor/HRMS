using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace hrms_be_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LgaController : BaseController
    {
        private readonly ILgaService _lgaService;

        public LgaController(ILgaService lgaService)
        {
            _lgaService = lgaService;
        }

        [HttpGet("GetLgas")]
        [ProducesResponseType(typeof(ExecutedResult<List<LgaVm>>), 200)]
        public async Task<IActionResult> GetLgas(int StateId)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _lgaService.GetLgas(StateId, accessToken, claim, RemoteIpAddress, RemotePort));
        }
    }
}
