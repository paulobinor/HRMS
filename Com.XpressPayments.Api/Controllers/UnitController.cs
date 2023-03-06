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
    public class UnitController : ControllerBase
    {
        private readonly ILogger<UnitController> _logger;
        private readonly IUnitService _unitService;

        public UnitController(ILogger<UnitController> logger, IUnitService unitService)
        {
            _logger = logger;
            _unitService = unitService;
        }

        [HttpPost("CreateUnit")]
        [Authorize]
        public async Task<IActionResult> CreateUnit([FromBody] CreateUnitDTO UnitDto)
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

                return Ok(await _unitService.CreateUnit(UnitDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : CreateUnit ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : CreateUnit ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("UpdateUnit")]
        [Authorize]
        public async Task<IActionResult> UpdateUnit([FromBody] UpdateUnitDTO updateDto)
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

                return Ok(await _unitService.UpdateUnit(updateDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : UpdateUnit ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : UpdateUnit ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPut("DeleteUnit")]
        [Authorize]
        public async Task<IActionResult> DeleteUnit([FromBody] DeleteUnitDTO deleteDto)
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

                return Ok(await _unitService.DeleteUnit(deleteDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DeleteUnit ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DeleteUnit ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetAllActiveUnit")]
        public async Task<IActionResult> GetAllActiveUnit()
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

                return Ok(await _unitService.GetAllActiveUnit(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllActiveUnit ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllActiveUnit ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }


        [Authorize]
        [HttpGet("GetAllUnit")]
        public async Task<IActionResult> GetAllUnit()
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

                return Ok(await _unitService.GetAllUnit(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllUnit ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllUnit ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetUnitbyId")]
        public async Task<IActionResult> GetUnitbyId(int UnitID)
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

                return Ok(await _unitService.GetUnitById(UnitID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetUnitbyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetUnitbyId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }
    }
}
