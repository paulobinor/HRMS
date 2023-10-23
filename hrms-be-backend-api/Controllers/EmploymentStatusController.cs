using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
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
    public class EmploymentStatusController : BaseController
    {
        private readonly ILogger<EmploymentStatusController> _logger;
        private readonly IEmploymentStatusService _employmentStatusService;

        public EmploymentStatusController(ILogger<EmploymentStatusController> logger, IEmploymentStatusService employmentStatusService)
        {
            _logger = logger;
            _employmentStatusService = employmentStatusService;
        }

        [HttpPost("CreateEmploymentStatus")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> CreateEmploymentStatus(CreateEmploymentStatusDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _employmentStatusService.CreateEmploymentStatus(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("UpdateEmploymentStatus")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> UpdateEmploymentStatus(UpdateEmploymentStatusDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _employmentStatusService.UpdateEmploymentStatus(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("DeleteEmploymentStatus")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> DeleteEmploymentStatus(DeleteEmploymentStatusDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _employmentStatusService.DeleteEmploymentStatus(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetEmploymentStatus")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<CompanyVm>>), 200)]
        public async Task<IActionResult> GetEmploymentStatus([FromQuery] PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _employmentStatusService.GetEmploymentStatus(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetEmploymentStatusDeleted")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<CompanyVm>>), 200)]
        public async Task<IActionResult> GetEmploymentStatusDeleted([FromQuery] PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _employmentStatusService.GetEmploymentStatusDeleted(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetEmploymentStatusById")]
        [ProducesResponseType(typeof(ExecutedResult<EmploymentStatusVm>), 200)]
        public async Task<IActionResult> GetEmploymentStatusById([FromQuery] long EmploymentStatusId)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _employmentStatusService.GetEmploymentStatusById(EmploymentStatusId, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetEmploymentStatusByName")]
        [ProducesResponseType(typeof(ExecutedResult<EmploymentStatusVm>), 200)]
        public async Task<IActionResult> GetEmploymentStatusByName([FromQuery] string EmploymentStatusName)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _employmentStatusService.GetEmploymentStatusByName(EmploymentStatusName, accessToken, claim, RemoteIpAddress, RemotePort));
        }
    }
}
