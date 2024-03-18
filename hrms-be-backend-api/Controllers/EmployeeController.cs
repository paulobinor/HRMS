using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace hrms_be_backend_api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class EmployeeController : BaseController
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeService _EmployeeService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService EmployeeService)
        {
            _logger = logger;
            _EmployeeService = EmployeeService;
        }

        [HttpPost("CreateEmployeeBasis")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> CreateEmployeeBasis(CreateEmployeeBasisDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _EmployeeService.CreateEmployeeBasis(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("UpdateEmployeeCompensation")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> UpdateEmployeeCompensation(UpdateEmployeeCompensationDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _EmployeeService.UpdateEmployeeCompensation(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("UpdateEmployeePersonalInfo")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> UpdateEmployeePersonalInfo(UpdateEmployeePersonalInfoDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _EmployeeService.UpdateEmployeePersonalInfo(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("UpdateEmployeeContactDetails")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> UpdateEmployeeContactDetails(UpdateEmployeeContactDetailsDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _EmployeeService.UpdateEmployeeContactDetails(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("UpdateEmployeeEduBackground")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> UpdateEmployeeEduBackground(UpdateEmployeeEduBackgroundDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _EmployeeService.UpdateEmployeeEduBackground(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("UpdateEmployeeProfesionalBackground")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> UpdateEmployeeProfesionalBackground(UpdateEmployeeProfBackgroundDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _EmployeeService.UpdateEmployeeProfesionalBackground(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("UpdateEmployeeReference")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> UpdateEmployeeReference(UpdateEmployeeReferenceDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _EmployeeService.UpdateEmployeeReference(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("UpdateEmployeeBankDetails")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> UpdateEmployeeBankDetails(UpdateEmployeeBankDetailsDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _EmployeeService.UpdateEmployeeBankDetails(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }

        [HttpPost("CreateEmployeeBulkUpload")]
        //[Authorize]
        public async Task<IActionResult> CreateEmployeeBulkUpload(IFormFile payload)
        {
            var response = new BaseResponse();
            try
            {
                var accessToken = Request.Headers["Authorization"];
                var requester = new RequesterInfo
                {
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString(),
                    AccessToken = accessToken.ToString().Replace("bearer", "").Trim(),
            };

                return Ok(await _EmployeeService.CreateEmployeeBulkUpload(payload, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : CreateUserBulkUpload ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : CreateUserBulkUpload ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }


        [HttpPost("UpdateEmployeeBasis")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> UpdateEmployeeBasis(UpdateEmployeeBasisDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _EmployeeService.UpdateEmployeeBasis(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("ApproveEmployee")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> ApproveEmployee(long EmployeeId)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _EmployeeService.ApproveEmployee(EmployeeId, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("DisapproveEmployee")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> DisapproveEmployee(long EmployeeId, string Comment)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _EmployeeService.DisapproveEmployee(EmployeeId, Comment, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("DeleteEmployee")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> DeleteEmployee(long EmployeeId, string Comment)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _EmployeeService.DeleteEmployee(EmployeeId, Comment, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("CheckEmployeeStaffId")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> CheckEmployeeStaffId(string StaffId)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _EmployeeService.CheckEmployeeStaffId(StaffId, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetEmployees")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<EmployeeVm>>), 200)]
        public async Task<IActionResult> GetEmployees([FromQuery] PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _EmployeeService.GetEmployees(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetEmployeesPending")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<EmployeeVm>>), 200)]
        public async Task<IActionResult> GetEmployeesPending([FromQuery] PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _EmployeeService.GetEmployeesPending(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetEmployeesApproved")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<EmployeeVm>>), 200)]
        public async Task<IActionResult> GetEmployeesApproved([FromQuery] PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _EmployeeService.GetEmployeesApproved(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetEmployeesDisapproved")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<EmployeeVm>>), 200)]
        public async Task<IActionResult> GetEmployeesDisapproved([FromQuery] PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _EmployeeService.GetEmployeesDisapproved(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetEmployeesDeleted")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<EmployeeVm>>), 200)]
        public async Task<IActionResult> GetEmployeesDeleted([FromQuery] PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _EmployeeService.GetEmployeesDeleted(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetEmployeeById")]
        [ProducesResponseType(typeof(ExecutedResult<EmployeeSindgleVm>), 200)]
        public async Task<IActionResult> GetEmployeeById([FromQuery] long EmployeeId)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _EmployeeService.GetEmployeeById(EmployeeId, accessToken, claim, RemoteIpAddress, RemotePort));
        }
    }
}
