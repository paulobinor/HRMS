using hrms_be_backend_business.ILogic;
using hrms_be_backend_business.Logic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static hrms_be_backend_business.Logic.CompanyAppModuleService;

namespace hrms_be_backend_api.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DepartmentalModulesController : BaseController
    {
        private readonly ILogger<DepartmentalModulesController> _logger;
        private readonly IDepartmentalModulesService _departmentalModulesService;

        public DepartmentalModulesController(ILogger<DepartmentalModulesController> logger, IDepartmentalModulesService departmentalModulesService)
        {
            _logger = logger;
            _departmentalModulesService = departmentalModulesService;
        }

       

        [HttpGet("GetDepartmetalAppModuleCount")]
        public async Task<IActionResult> GetDepartmetalAppModuleCount()
        {
            
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().ToLower().Replace("bearer", "").Trim();
            return this.CustomResponse(await _departmentalModulesService.GetDepartmentalAppModuleCount(accessToken, claim, RemoteIpAddress, RemotePort));
            
            
        }

        [HttpGet("GetDepartmentAppModuleByStatus/{status}")]
        public async Task<IActionResult> GetDepartmentAppModuleByStatus(GetByStatus status)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().ToLower().Replace("bearer", "").Trim();
            return this.CustomResponse(await _departmentalModulesService.GetDepartmentAppModuleStatus(status, accessToken, claim, RemoteIpAddress, RemotePort));
           
        }

        [HttpGet("GetDepartmentAppModuleByDepartmentID/{departmentID}")]
        public async Task<IActionResult> GetDepartmentAppModuleByDepartmentID(long departmentID)
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().ToLower().Replace("bearer", "").Trim();

            return this.CustomResponse(await _departmentalModulesService.GetDepartmentalAppModuleByDepartmentID(departmentID, accessToken, claim, RemoteIpAddress, RemotePort));
           
        }



        [HttpGet("GetPendingDepartmentalAppModule")]
        public async Task<IActionResult> GetPendingDepartmentalAppModule()
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().ToLower().Replace("bearer", "").Trim();

            return this.CustomResponse(await _departmentalModulesService.GetPendingDepartmentalAppModule(accessToken, claim, RemoteIpAddress, RemotePort));
            
        }

        [HttpPost("CreateDepartmentalAppModule")]
        public async Task<IActionResult> CreateDepartmentalAppModule(CreateDepartmentalModuleDTO request)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().ToLower().Replace("bearer", "").Trim();
            return this.CustomResponse(await _departmentalModulesService.CreateDepartmentalAppModule(request, accessToken, claim, RemoteIpAddress, RemotePort));
           
        }

        [HttpPost("ApproveDepartmentAppModule")]
        public async Task<IActionResult> ApproveDepartmentAppModule(ApproveDepartmentalModules request)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().ToLower().Replace("bearer", "").Trim();
            return this.CustomResponse(await _departmentalModulesService.ApproveDepartmentalAppModule(request, accessToken, claim, RemoteIpAddress, RemotePort));
            
        }

        [HttpPost("DisapproveDepartmentalAppModule")]
        public async Task<IActionResult> DisapproveDepartmentalAppModule(ApproveDepartmentalModules request)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().ToLower().Replace("bearer", "").Trim();

            return this.CustomResponse(await _departmentalModulesService.DisapproveDepartmentalAppModule(request, accessToken, claim, RemoteIpAddress, RemotePort));
           
        }

        [HttpGet("DeleteDepartmentAppModule/{departmentAppModuleID}")]
        public async Task<IActionResult> DeleteDepartmentalAppModule(long departmentAppModuleID)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().ToLower().Replace("bearer", "").Trim();

            return this.CustomResponse(await _departmentalModulesService.DeleteDepartmentAppModule(departmentAppModuleID, accessToken, claim, RemoteIpAddress, RemotePort));
            
        }
        [HttpGet("DepartmentalAppModuleActivationSwitch/{departmentAppModuleID}")]
        public async Task<IActionResult> DepartmentalAppModuleActivationSwitch(long departmentAppModuleID)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().ToLower().Replace("bearer", "").Trim();

            return this.CustomResponse(await _departmentalModulesService.DepartmentAppModuleActivationSwitch(departmentAppModuleID, accessToken, claim, RemoteIpAddress, RemotePort));
            
        }

    }
}
