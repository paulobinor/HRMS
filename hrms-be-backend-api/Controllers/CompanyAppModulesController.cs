using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using static hrms_be_backend_business.Logic.CompanyAppModuleService;

namespace hrms_be_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyAppModulesController : BaseController
    {
        private readonly ILogger<CompanyAppModulesController> _logger;
        private readonly ICompanyAppModuleService _companyAppModuleService;

        public CompanyAppModulesController(ILogger<CompanyAppModulesController> logger, ICompanyAppModuleService companyAppModuleService)
        {
            _logger = logger;
            _companyAppModuleService = companyAppModuleService;
        }

        [HttpGet("GetAllAppModules")]
        public async Task<IActionResult> GetAllAppModules()
        {
            var requester = new RequesterInfo
            {
                Username = this.User.Claims.ToList()[1].Value,
                //UserId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                //RoleId = Convert.ToInt64(this.User.Claims.ToList()[5].Value),
                IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                Port = Request.HttpContext.Connection.RemotePort.ToString()
            };

            return this.CustomResponse(await _companyAppModuleService.GetAllAppModules(requester));

        }

        [HttpGet("GetCompanyAppModuleCount")]
        public async Task<IActionResult> GetCompanyAppModuleCount()
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _companyAppModuleService.GetCompanyAppModuleCount(accessToken, claim, RemoteIpAddress, RemotePort));

        }

        [HttpGet("GetCompanyAppModuleByCompanyID/{companyID}")]
        public async Task<IActionResult> GetCompanyAppModuleByCompanyID(long companyID)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _companyAppModuleService.GetCompanyAppModuleByCompanyID(companyID, accessToken, claim, RemoteIpAddress, RemotePort));

        }

        [HttpGet("GetCompanyAppModuleBySatus/{status}")]
        public async Task<IActionResult> GetCompanyAppModuleByCompanyID(GetByStatus status)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _companyAppModuleService.GetCompanyAppModuleStatus(status, accessToken, claim, RemoteIpAddress, RemotePort));

        }


        [HttpGet("GetPendingCompanyAppModule")]
        public async Task<IActionResult> GetPendingCompanyAppModule()
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _companyAppModuleService.GetPendingCompanyAppModule(accessToken, claim, RemoteIpAddress, RemotePort));

        }

        [HttpPost("CreateCompanyAppModule")]
        public async Task<IActionResult> CreateCompanyAppModule(CreateCompanyAppModuleDTO request)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _companyAppModuleService.CreateCompanyAppModule(request, accessToken, claim, RemoteIpAddress, RemotePort));

        }

        [HttpPost("ApproveCompanyAppModule")]
        public async Task<IActionResult> ApproveCompanyAppModule(ApproveCompanyAppModulesRequest request)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _companyAppModuleService.ApproveCompanyAppModule(request, accessToken, claim, RemoteIpAddress, RemotePort));

        }

        [HttpPost("DisapproveCompanyAppModule")]
        public async Task<IActionResult> DisapproveCompanyAppModule(ApproveCompanyAppModulesRequest request)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _companyAppModuleService.DisapproveCompanyAppModule(request, accessToken, claim, RemoteIpAddress, RemotePort));

        }

        [HttpGet("DeleteCompanyAppModule/{companyAppModuleID}")]
        public async Task<IActionResult> DeleteCompanyAppModule(long companyAppModuleID)
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _companyAppModuleService.DeleteCompanyAppModule(companyAppModuleID, accessToken, claim, RemoteIpAddress, RemotePort));

        }
        [HttpGet("CompanyAppModuleActivationSwitch/{companyAppModuleID}")]
        public async Task<IActionResult> CompanyAppModuleActivationSwitch(long companyAppModuleID)
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _companyAppModuleService.CompanyAppModuleActivationSwitch(companyAppModuleID, accessToken, claim, RemoteIpAddress, RemotePort));

        }
    }
}
