<<<<<<< HEAD:Com.XpressPayments.Api/OnBoardingModuleController/Controllers/HODController.cs
﻿//using Com.XpressPayments.Bussiness.Services.ILogic;
//using Com.XpressPayments.Bussiness.Services.Logic;
//using Com.XpressPayments.Data.DTOs;
//using Com.XpressPayments.Data.Enums;
//using Com.XpressPayments.Data.GenericResponse;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System.Linq;
//using System.Threading.Tasks;
//using System;
//using Microsoft.AspNetCore.Http;
//using OfficeOpenXml;
//using System.Collections.Generic;

//namespace Com.XpressPayments.Api.Controllers
//{

//    [Route("api/[controller]")]
//    [ApiController]
//    public class HODController : ControllerBase
//    {
//        private readonly ILogger<HODController> _logger;
//        private readonly IHODService _HODService;

//        public HODController(ILogger<HODController> logger, IHODService HodService)
//        {
//            _logger = logger;
//            _HODService = HodService;
//        }

//        [HttpPost("CreateHOD")]
//        [Authorize]
//        public async Task<IActionResult> CreateHOD([FromBody] CreateHodDTO hodDto)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.LocalPort.ToString()
//                };

//                return Ok(await _HODService.CreateHOD(hodDto, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : CreateHOD ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : CreateHOD ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }
//        }

//        [HttpPost("CreateHODBulkUpload")]
//        [Authorize]
//        public async Task<IActionResult> CreateHODBulkUpload(IFormFile payload)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.LocalPort.ToString()
//                };

//                return Ok(await _HODService.CreateHODBulkUpload(payload, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : CreateHODBulkUpload ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : CreateHODBulkUpload ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }
//        }

//        [HttpPost("UpdateHOD")]
//        [Authorize]
//        public async Task<IActionResult> UpdateHOD([FromBody] UpdateHodDTO updateDto)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.LocalPort.ToString()
//                };

//                return Ok(await _HODService.UpdateHOD(updateDto, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : UpdateHOD ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : UpdateHOD ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }
//        }

//        [HttpPost("DeleteHOD")]
//        [Authorize]
//        public async Task<IActionResult> DeleteHOD([FromBody] DeleteHodDTO deleteDto)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.LocalPort.ToString()
//                };

//                return Ok(await _HODService.DeleteHOD(deleteDto, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : DeleteHOD ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : DeleteHOD ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }
//        }

//        [Authorize]
//        [HttpGet("GetAllActiveHOD")]
//        public async Task<IActionResult> GetAllActiveHOD()
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.LocalPort.ToString()
//                };

//                return Ok(await _HODService.GetAllActiveHOD(requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : GetAllActiveHOD ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllActiveHOD ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }

//        }

//        [Authorize]
//        [HttpGet("GetAllHOD")]
//        public async Task<IActionResult> GetAllHOD()
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.LocalPort.ToString()
//                };

//                return Ok(await _HODService.GetAllHOD(requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : GetAllHOD ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllHOD ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }
//        }

//        [Authorize]
//        [HttpGet("GetHODbyId")]
//        public async Task<IActionResult> GetHODbyId(int HodID)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.LocalPort.ToString()
//                };

//                return Ok(await _HODService.GetHODbyId(HodID, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : GetHODbyId ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetHODbyId ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }

//        }


//        [Authorize]
//        [HttpGet("GetHODbyCompanyId")]
//        public async Task<IActionResult> GetHODbyCompanyId(long CompanyID)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.LocalPort.ToString()
//                };

//                return Ok(await _HODService.GetHODbyCompanyId(CompanyID, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : GetHODbyCompanyId ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetHODbyCompanyId ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }

//        }

//    }
//}
=======
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Business.Services.ILogic;
using XpressHRMS.Data.DTO;
using XpressHRMS.Data.Enums;

namespace XpressHRMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HodController : BaseController
    {
        private readonly IHodService _HODService;
        public HodController(IHodService HODService)
        {
            _HODService = HODService;
        }


        [HttpPost("CreateHOD")]
        public async Task<IActionResult> CreateHOD([FromBody] CreateHodDTO createHOD)
        {
            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {

                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                string CreatedBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                string companyid = claimsIdentity.FindFirst(ClaimTypes.SerialNumber)?.Value;
                createHOD.CreatedBy = CreatedBy;
                createHOD.CompanyID = companyid;

                return this.CustomResponse(await _HODService.CreateHOD(createHOD, RemoteIpAddress, RemotePort));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPut("UpdateHOD")]
        public async Task<IActionResult> UpdatePosition([FromBody] UpdateHodDTO UpdateHOD)
        {
            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {

                return this.CustomResponse(await _HODService.UpdateHOD(UpdateHOD, RemoteIpAddress, RemotePort));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpDelete("DeleteHOD")]
        public async Task<IActionResult> DeleteHOD(DelHodDTO DelHOD)
        {
            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {

                return this.CustomResponse(await _HODService.DeleteHOD(DelHOD, RemoteIpAddress, RemotePort));

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPost("ActivateHOD")]
        public async Task<IActionResult> ActivateHOD(EnableHodDTO enable)
        {
            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {

                return this.CustomResponse(await _HODService.ActivateHOD(enable, RemoteIpAddress, RemotePort));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost("DisableHOD")]
        public async Task<IActionResult> DisableHOD(DisableHodDTO disable)
        {
            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {

                return this.CustomResponse(await _HODService.DisableHOD(disable, RemoteIpAddress, RemotePort));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("GetAllHOD")]
        public async Task<IActionResult> GetAllHOD(string CompanyID)
        {
            try
            {
                return this.CustomResponse(await _HODService.GetAllHOD(CompanyID));

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpGet("GetAllHODByID")]
        public async Task<IActionResult> GetAllHODByID(string CompanyID, int HodID, int DepartmentID)
        {
            try
            {
                return this.CustomResponse(await _HODService.GetHODByID(CompanyID, HodID, DepartmentID));

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
>>>>>>> origin/origin/clintonDev:XpressHRMS/Controllers/HODController.cs
