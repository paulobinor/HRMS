using Com.XpressPayments.Bussiness.Services.ILogic;
using Com.XpressPayments.Bussiness.Services.Logic;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.GenericResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Com.XpressPayments.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HODController : ControllerBase
    {
        private readonly ILogger<HODController> _logger;
        private readonly IHODService _HODService;

        public HODController(ILogger<HODController> logger, IHODService HodService)
        {
            _logger = logger;
            _HODService = HodService;
        }

        [HttpPost("CreateHOD")]
        [Authorize]
        public async Task<IActionResult> CreateHOD([FromBody] CreateHodDTO hodDto)
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

                return Ok(await _HODService.CreateHOD(hodDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : CreateHOD ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : CreateHOD ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("UpdateHOD")]
        [Authorize]
        public async Task<IActionResult> UpdateHOD([FromBody] UpdateHodDTO updateDto)
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

                return Ok(await _HODService.UpdateHOD(updateDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : UpdateHOD ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : UpdateHOD ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPut("DeleteHOD")]
        [Authorize]
        public async Task<IActionResult> DeleteHOD([FromBody] DeleteHodDTO deleteDto)
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

                return Ok(await _HODService.DeleteHOD(deleteDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DeleteHOD ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DeleteHOD ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetAllActiveHOD")]
        public async Task<IActionResult> GetAllActiveHOD()
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

                return Ok(await _HODService.GetAllActiveHOD(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllActiveHOD ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllActiveHOD ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [Authorize]
        [HttpGet("GetAllHOD")]
        public async Task<IActionResult> GetAllHOD()
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

                return Ok(await _HODService.GetAllHOD(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllActiveHOD ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllActiveHOD ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetHODbyId")]
        public async Task<IActionResult> GetHODbyId(int HodID)
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

                return Ok(await _HODService.GetHODbyId(HodID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetHODbyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetHODbyId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }
    }
}
