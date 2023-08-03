using Com.XpressPayments.Data.DTOs.Account;
using Com.XpressPayments.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using Com.XpressPayments.Common.ViewModels;
using Microsoft.Extensions.Logging;
using Com.XpressPayments.Bussiness.ExitModuleService.Services.ILogic;
using Com.XpressPayments.Bussiness.Services.Logic;
using Com.XpressPayments.Bussiness.Services.ILogic;
using Com.XpressPayments.Data.GenericResponse;
using Microsoft.AspNetCore.Authorization;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data;

namespace Com.XpressPayments.Api.ExitModuleController.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ResignationController : ControllerBase
    {
        private readonly ILogger<ResignationController> _logger;
        private readonly IResignationService _resignationService;

        public ResignationController(ILogger<ResignationController> logger, IResignationService resignationService)
        {
            _logger = logger;
            _resignationService = resignationService;
        }
        [HttpPost]
        public async Task<IActionResult> SubmitResignation( ResignationRequestVM request)
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

                return Ok(await _resignationService.SubmitResignation(requester, request));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllGender() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllGender() ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }


        [HttpPost]
        [Route("UploadResignationLetter")]
        public async Task<IActionResult> UploadFile_(IFormFile letter)
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

                return Ok(await _resignationService.UploadLetter(letter));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllGender() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllGender() ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }


        //[Authorize]
        [HttpGet]
        [Route("GetResignationByID/{resignationID}")]
        public async Task<IActionResult> GetResignationByID(long resignationID)
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

                return Ok(await _resignationService.GetResignationByID(resignationID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetResignationByID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetResignationByID ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        //[Authorize]
        [HttpGet]
        [Route("GetResignationByUserID/{userID}")]
        public async Task<IActionResult> GetResignationByUserID(long userID)
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

                return Ok(await _resignationService.GetResignationByUserID(userID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetResignationByUserID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetResignationByUserID ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        //[Authorize]
        [HttpGet]
        [Route("GetResignationByCompanyID/{companyId}/{isApproved}")]
        public async Task<IActionResult> GetResignationByCompanyID(long companyId , bool isApproved)
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

                return Ok(await _resignationService.GetResignationByCompanyID(companyId, isApproved,requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetResignationByCompanyID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetResignationByCompanyID ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        
        //[Authorize]
        [HttpPost]
        [Route("DeleteResignation")]
        public async Task<IActionResult> DeleteResignation([FromBody] DeleteResignationDTO request)
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

                return Ok(await _resignationService.DeleteResignation(request, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DeleteResignation ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DeleteResignation ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("GetPendingResignationByUserID/{userID}")]
        public async Task<IActionResult> GetPendingResignationByUserID(long userID)
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

                return Ok(await _resignationService.GetResignationByUserID(userID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetPendingResignationByUserID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetPendingResignationByUserID ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        //[Authorize]
        [HttpPost]
        [Route("ApprovePendingResignation")]
        public async Task<IActionResult> ApprovePendingResignation([FromBody] ApprovePendingResignationDTO request)
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

                return Ok(await _resignationService.ApprovePendingResignation(request, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : ApproveEmp ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : ApproveEmp ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [HttpPost]
        [Route("DisapprovePendingResignation")]
        public async Task<IActionResult> DisapprovePendingResignation([FromBody] DisapprovePendingResignation request)
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

                return Ok(await _resignationService.DisapprovePendingResignation(request, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : ApproveEmp ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : ApproveEmp ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }
    }
}
