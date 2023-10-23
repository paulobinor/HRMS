﻿using hrms_be_backend_business.ILogic;
using hrms_be_backend_business.Logic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
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

        [HttpPost("CreateEmployeeBulkUpload/{companyID}")]
        //[Authorize]
        public async Task<IActionResult> CreateEmployeeBulkUpload(IFormFile payload, long companyID)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                     Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _EmployeeService.CreateEmployeeBulkUpload(payload, companyID, requester));
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
        [HttpGet("GetEmployees")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<EmployeeVm>>), 200)]
        public async Task<IActionResult> GetEmployees(PaginationFilter filter)
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
        [HttpGet("GetEmployeesApproved")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<EmployeeVm>>), 200)]
        public async Task<IActionResult> GetEmployeesApproved(PaginationFilter filter)
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
        public async Task<IActionResult> GetEmployeesDisapproved(PaginationFilter filter)
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
        public async Task<IActionResult> GetEmployeesDeleted(PaginationFilter filter)
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
        [ProducesResponseType(typeof(ExecutedResult<EmployeeFullVm>), 200)]
        public async Task<IActionResult> GetEmployeeById(long EmployeeId)
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
