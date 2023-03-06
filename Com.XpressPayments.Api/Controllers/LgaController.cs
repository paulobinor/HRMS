using Com.XpressPayments.Bussiness.Services.ILogic;
using Com.XpressPayments.Bussiness.Services.Logic;
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
    public class LgaController : ControllerBase
    {
        private readonly ILogger<LgaController> _logger;
        private readonly ILgaService _IgaService;

        public LgaController(ILogger<LgaController> logger, ILgaService IgaService)
        {
            _logger = logger;
            _IgaService = IgaService;
        }

        [Authorize]
        [HttpGet("GetAllLga")]
        public async Task<IActionResult> GetAllState(int StateID)
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

                return Ok(await _IgaService.GetAllLga(StateID,requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllState ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllState ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetLgabyId")]
        public async Task<IActionResult> GetLgabyStateId(int StateID, int LGAID)
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

                return Ok(await _IgaService.GetLgaByStateId(StateID, LGAID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetLgabyStateId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetLgabyStateId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }
    }
}
