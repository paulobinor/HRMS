﻿using hrms_be_backend_business.ILogic;
using hrms_be_backend_business.Logic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace hrms_be_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("CreateBackOfficeUser")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> CreateBackOfficeUser(CreateUserDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();            
            return this.CustomResponse(await _userService.CreateBackOfficeUser(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetUsers")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<UserVm>>), 200)]
        public async Task<IActionResult> GetUsers(PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _userService.GetUsers(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetUsersBackOffice")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<UserVm>>), 200)]
        public async Task<IActionResult> GetUsersBackOffice(PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _userService.GetUsersBackOffice(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetUsersApprovedBackOffice")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<UserVm>>), 200)]
        public async Task<IActionResult> GetUsersApprovedBackOffice(PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _userService.GetUsersApprovedBackOffice(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetUsersDisapprovedBackOffice")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<UserVm>>), 200)]
        public async Task<IActionResult> GetUsersDisapprovedBackOffice(PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _userService.GetUsersDisapprovedBackOffice(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetUsersDeapctivatedBackOffice")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<UserVm>>), 200)]
        public async Task<IActionResult> GetUsersDeapctivatedBackOffice(PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _userService.GetUsersDeapctivatedBackOffice(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetUsersByCompany")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<UserVm>>), 200)]
        public async Task<IActionResult> GetUsersByCompany(PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _userService.GetUsersByCompany(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetUsersApprovedByCompany")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<UserVm>>), 200)]
        public async Task<IActionResult> GetUsersApprovedByCompany(PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _userService.GetUsersApprovedByCompany(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetUsersDisapprovedByCompany")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<UserVm>>), 200)]
        public async Task<IActionResult> GetUsersDisapprovedByCompany(PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _userService.GetUsersDisapprovedByCompany(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetUsersDeactivatedByCompany")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<UserVm>>), 200)]
        public async Task<IActionResult> GetUsersDeactivatedByCompany(PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _userService.GetUsersDeactivatedByCompany(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetUserById")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<UserVm>>), 200)]
        public async Task<IActionResult> GetUserById(long UserId)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _userService.GetUserById(UserId, accessToken, claim, RemoteIpAddress, RemotePort));
        }
    }
}
