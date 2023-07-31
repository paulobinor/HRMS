<<<<<<< HEAD:Com.XpressPayments.Api/OnBoardingModuleController/Controllers/LgaController.cs
﻿using Com.XpressPayments.Bussiness.Services.ILogic;
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
        public async Task<IActionResult> GetAllState(long StateID)
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
        [HttpGet("GetLgabyStateId")]
        public async Task<IActionResult> GetLgabyStateId(long StateID)
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

                return Ok(await _IgaService.GetLgaByStateId(StateID, requester));
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
    public class LgaController : BaseController
    {
        private readonly ILgaService _LgaService;
        public LgaController(ILgaService LgaService)
        {
            _LgaService = LgaService;
        }

        [HttpGet("GetAllStateById")]
        public async Task<IActionResult> GetAllLgaByStateId(int StateID, int LGAID)
        {
            try
            {
                return this.CustomResponse(await _LgaService.GetAllLGAByState(StateID, LGAID));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("GetAllLga")]
        public async Task<IActionResult> GetAllLga(int StateID)
        {
            try
            {
                return this.CustomResponse(await _LgaService.GetAllLGA(StateID));

            }
            catch (Exception ex)
            {
                return null;
            }
        }
>>>>>>> origin/origin/clintonDev:XpressHRMS/Controllers/LgaController.cs
    }
}
