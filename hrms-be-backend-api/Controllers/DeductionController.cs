using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace hrms_be_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeductionController : BaseController
    {
        private readonly IDeductionsService _deductionsService;
        public DeductionController(IDeductionsService deductionsService)
        {
            _deductionsService = deductionsService;
        }


        [HttpPost("CreateDeduction")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> CreateDeduction(DeductionCreateDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _deductionsService.CreateDeduction(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("UpdateDeduction")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> UpdateDeduction(DeductionUpdateDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _deductionsService.UpdateDeduction(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("DeleteDeductions")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> DeleteDeductions(long DeductionsId, string Comments)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _deductionsService.DeleteDeduction(DeductionsId, Comments, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetDeductions")]
        [ProducesResponseType(typeof(ExecutedResult<IEnumerable<DeductionView>>), 200)]
        public async Task<IActionResult> GetDeductions()
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _deductionsService.GetDeductions(accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetDeductionById")]
        [ProducesResponseType(typeof(ExecutedResult<DeductionView>), 200)]
        public async Task<IActionResult> GetDeductionById(long Id)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _deductionsService.GetDeductionById(Id, accessToken, claim, RemoteIpAddress, RemotePort));
        }
    }
}
