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
    public class EmployeeTypeController : BaseController
    {

        private readonly ILogger<EmployeeTypeController> _logger;
        private readonly IEmployeeTypeService _employeeTypeService;

        public EmployeeTypeController(ILogger<EmployeeTypeController> logger, IEmployeeTypeService employeeTypeService)
        {
            _logger = logger;
            _employeeTypeService = employeeTypeService;
        }

        [HttpPost("CreateEmployeeType")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> CreateEmployeeType(CreateEmployeeTypeDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _employeeTypeService.CreateEmployeeType(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("UpdateEmployeeType")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> UpdateEmployeeType(UpdateEmployeeTypeDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _employeeTypeService.UpdateEmployeeType(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("DeleteEmployeeType")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> DeleteEmployeeType(DeleteEmployeeTypeDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _employeeTypeService.DeleteEmployeeType(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetEmployeeTypes")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<CompanyVm>>), 200)]
        public async Task<IActionResult> GetEmployeeTypes([FromQuery] PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _employeeTypeService.GetEmployeeTypes(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetEmployeeTypesDeleted")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<CompanyVm>>), 200)]
        public async Task<IActionResult> GetEmployeeTypesDeleted([FromQuery] PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _employeeTypeService.GetEmployeeTypesDeleted(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetEmployeeTypeById")]
        [ProducesResponseType(typeof(ExecutedResult<EmployeeTypeVm>), 200)]
        public async Task<IActionResult> GetEmployeeTypeById([FromQuery] long EmployeeTypeId)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _employeeTypeService.GetEmployeeTypeById(EmployeeTypeId, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetEmployeeTypeByName")]
        [ProducesResponseType(typeof(ExecutedResult<EmployeeTypeVm>), 200)]
        public async Task<IActionResult> GetEmployeeTypeByName([FromQuery] string EmployeeTypeName)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _employeeTypeService.GetEmployeeTypeByName(EmployeeTypeName, accessToken, claim, RemoteIpAddress, RemotePort));
        }
    }
}
