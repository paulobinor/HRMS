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
    public class CompanyController : BaseController
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly ICompanyService _CompanyService;

        public CompanyController(ILogger<CompanyController> logger, ICompanyService CompanyService)
        {
            _logger = logger;
            _CompanyService = CompanyService;
        }

        [HttpPost("CreateCompany")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> CreateCompany(CompanyCreateDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _CompanyService.CreateCompany(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }

        [HttpPost("UpdateCompany")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> UpdateCompany(CompanyUpdateDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _CompanyService.UpdateCompany(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("DeleteCompany")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> DeleteCompany(long CompanyId, string ReasonToDelete)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _CompanyService.DeleteCompany(CompanyId, ReasonToDelete, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("ApproveCompany")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> ApproveCompany(long CompanyId, string ReasonToDelete)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _CompanyService.ApproveCompany(CompanyId, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetCompanies")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<CompanyVm>>), 200)]
        public async Task<IActionResult> GetCompanies([FromQuery] PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _CompanyService.GetCompanies(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetCompaniesPending")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<CompanyVm>>), 200)]
        public async Task<IActionResult> GetCompaniesPending([FromQuery] PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _CompanyService.GetCompaniesPending(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetCompaniesActivated")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<CompanyVm>>), 200)]
        public async Task<IActionResult> GetCompaniesActivated([FromQuery] PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _CompanyService.GetCompaniesActivated(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetCompaniesDeactivated")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<CompanyVm>>), 200)]
        public async Task<IActionResult> GetCompaniesDeactivated([FromQuery] PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _CompanyService.GetCompaniesDeactivated(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetCompaniesPublicSector")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<CompanyVm>>), 200)]
        public async Task<IActionResult> GetCompaniesPublicSector([FromQuery] PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _CompanyService.GetCompaniesPublicSector(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetCompaniesPrivateSector")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<CompanyVm>>), 200)]
        public async Task<IActionResult> GetCompaniesPrivateSector([FromQuery] PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _CompanyService.GetCompaniesPrivateSector(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetCompanyById")]
        [ProducesResponseType(typeof(ExecutedResult<CompanyFullVm>), 200)]
        public async Task<IActionResult> GetCompanyById([FromQuery] long Id)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _CompanyService.GetCompanyById(Id, accessToken, claim, RemoteIpAddress, RemotePort));
        }
    }
}
