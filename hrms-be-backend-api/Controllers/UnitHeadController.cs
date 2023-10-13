﻿//using Com.XpressPayments.Bussiness.Services.ILogic;
//using Com.XpressPayments.Data.DTOs;
//using Com.XpressPayments.Data.Enums;
//using Com.XpressPayments.Data.GenericResponse;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace hrms_be_backend_api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UnitHeadController : ControllerBase
//    {
//        private readonly ILogger<UnitHeadController> _logger;
//        private readonly IUnitHeadService _unitHeadService;

//        public UnitHeadController(ILogger<UnitHeadController> logger, IUnitHeadService unitHeadService)
//        {
//            _logger = logger;
//            _unitHeadService = unitHeadService;
//        }

//        [HttpPost("CreateUnitHead")]
//        [Authorize]
//        public async Task<IActionResult> CreateUnitHead([FromBody] CreateUnitHeadDTO UnitHeadDto)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.RemotePort.ToString()
//                };

//                return Ok(await _unitHeadService.CreateUnitHead(UnitHeadDto, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : CreateUnitHead ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : CreateUnitHead ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }
//        }

//        [HttpPost("CreateUnitHeadBulkUpload")]
//        [Authorize]
//        public async Task<IActionResult> CreateUnitHeadBulkUpload(IFormFile payload)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.RemotePort.ToString()
//                };

//                return Ok(await _unitHeadService.CreateUnitHeadBulkUpload(payload, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : UnitHead ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : UnitHead ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }
//        }

//        [HttpPost("UpdateUnitHead")]
//        [Authorize]
//        public async Task<IActionResult> UpdateUnitHead([FromBody] UpdateUnitHeadDTO updateDto)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.RemotePort.ToString()
//                };

//                return Ok(await _unitHeadService.UpdateUnitHead(updateDto, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : UpdateUnitHead ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : UpdateUnitHead ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }
//        }

//        [HttpPost("DeleteUnitHead")]
//        [Authorize]
//        public async Task<IActionResult> DeleteUnitHead([FromBody] DeleteUnitHeadDTO deleteDto)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.RemotePort.ToString()
//                };

//                return Ok(await _unitHeadService.DeleteUnitHead(deleteDto, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : DeleteUnitHead ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : DeleteUnitHead ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }
//        }

//        [Authorize]
//        [HttpGet("GetAllActiveUnit")]
//        public async Task<IActionResult> GetAllActiveUnitHead()
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.RemotePort.ToString()
//                };

//                return Ok(await _unitHeadService.GetAllActiveUnitHead(requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : GetAllActiveUnitHead ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllActiveUnitHead ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }

//        }


//        [Authorize]
//        [HttpGet("GetAllUnitHead")]
//        public async Task<IActionResult> GetAllUnitHead()
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.RemotePort.ToString()
//                };

//                return Ok(await _unitHeadService.GetAllUnitHead(requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : GetAllUnitHead ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllUnitHead ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }
//        }

//        [Authorize]
//        [HttpGet("GetUnitHeadbyId")]
//        public async Task<IActionResult> GetUnitHeadbyId(int UnitHeadID)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.RemotePort.ToString()
//                };

//                return Ok(await _unitHeadService.GetUnitHeadById(UnitHeadID, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : GetUnitHeadbyId ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetUnitHeadbyId ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }

//        }

//        [Authorize]
//        [HttpGet("GetUnitHeadbyCompanyId")]
//        public async Task<IActionResult> GetUnitHeadbyCompanyId(long CompanyID)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.RemotePort.ToString()
//                };

//                return Ok(await _unitHeadService.GetUnitHeadbyCompanyId(CompanyID, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : GetUnitHeadbyCompanyId ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetUnitHeadbyCompanyId ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }

//        }


//    }
//}