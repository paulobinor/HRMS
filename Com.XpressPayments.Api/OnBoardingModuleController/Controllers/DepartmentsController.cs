﻿using AutoMapper;
using Com.XpressPayments.Bussiness.Services.ILogic;
using Com.XpressPayments.Bussiness.Services.Logic;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.DTOs.Account;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.XpressPayments.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ILogger<DepartmentController> _logger;
        private readonly IDepartmentService _DepartmentService;

        public DepartmentController(ILogger<DepartmentController> logger, IDepartmentService DepartmentService)
        {
            _logger = logger;
            _DepartmentService = DepartmentService;
        }

        [HttpPost("CreateDepartment")]
        [Authorize]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDto DepartmentDto)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _DepartmentService.CreateDepartment(DepartmentDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : CreateDepartment ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : CreateDepartment ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("CreateDepartmentBulkUpload")]
        [Authorize]
        public async Task<IActionResult> CreateDepartmentBulkUpload(IFormFile payload)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _DepartmentService.CreateDepartmentBulkUpload(payload, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : CreateDepartmentBulkUpload ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : CreateDepartmentBulkUpload ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }


        

        [HttpPost("UpdateDepartment")]
        [Authorize]
        public async Task<IActionResult> UpdateDepartment([FromBody] UpdateDepartmentDto updateDto)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _DepartmentService.UpdateDepartment(updateDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : UpdateDepartment ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : UpdateDepartment ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("DeleteDepartment")]
        [Authorize]
        public async Task<IActionResult> DeleteDepartment([FromBody] DeleteDepartmentDto deleteDto)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _DepartmentService.DeleteDepartment(deleteDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DeleteDepartment ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DeleteDepartment ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetAllActiveDepartments")]
        public async Task<IActionResult> GetAllActiveDepartments()
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _DepartmentService.GetAllActiveDepartments(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllActiveDepartments ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllActiveDepartments ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [Authorize]
        [HttpGet("GetAllDepartments")]
        public async Task<IActionResult> GetAllDepartments()
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _DepartmentService.GetAllDepartments(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllDepartments ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllDepartments ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetDepartmentbyId")]
        public async Task<IActionResult> GetDepartmentbyId(int DepartmentId)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _DepartmentService.GetDepartmentbyId(DepartmentId, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetDepartmentbyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetDepartmentbyId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [Authorize]
        [HttpGet("GetDepartmentbyCompanyId")]
        public async Task<IActionResult> GetDepartmentbyCompanyId(long companyId)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _DepartmentService.GetDepartmentbyCompanyId(companyId, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetDepartmentbyCompanyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetDepartmentbyCompanyId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }
    }
}