using hrms_be_backend_business.ILogic;
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
    public class EarningsController : BaseController
    {
        private readonly IEarningsService _earningsService;
        public EarningsController(IEarningsService earningsService)
        {
            _earningsService = earningsService;
        }


        [HttpPost("CreateEarning")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> CreateEarning(EarningsCreateDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _earningsService.CreateEarning(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }

        [HttpPost("UpdateEarning")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> UpdateEarning(EarningsUpdateDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _earningsService.UpdateEarning(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }

        [HttpPost("DeleteEarnings")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> DeleteEarnings(long EarningsId, string Comments)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _earningsService.DeleteEarnings(EarningsId, Comments, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetEarnings")]
        [ProducesResponseType(typeof(ExecutedResult<EarningsView>), 200)]
        public async Task<IActionResult> GetEarnings()
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _earningsService.GetEarnings(accessToken, claim, RemoteIpAddress, RemotePort));
        }
       

        [HttpPost("CreateEarningItem")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> CreateEarningItem(EarningsItemCreateDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _earningsService.CreateEarningItem(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("UpdateEarningItem")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> UpdateEarningItem(EarningsItemUpdateDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _earningsService.UpdateEarningItem(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("DeleteEarningsItem")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> DeleteEarningsItem(long EarningsItemId, string Comments)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _earningsService.DeleteEarningsItem(EarningsItemId, Comments, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetEarningsItem")]
        [ProducesResponseType(typeof(ExecutedResult<IEnumerable<EarningsItemVm>>), 200)]
        public async Task<IActionResult> GetEarningsItem()
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _earningsService.GetEarningsItem(accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetEarningsItemById")]
        [ProducesResponseType(typeof(ExecutedResult<EarningsItemVm>), 200)]
        public async Task<IActionResult> GetEarningsItemById(long Id)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _earningsService.GetEarningsItemById(Id, accessToken, claim, RemoteIpAddress, RemotePort));
        }
    }
}
