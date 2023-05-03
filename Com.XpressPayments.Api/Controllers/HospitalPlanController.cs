using Com.XpressPayments.Bussiness.Services.ILogic;
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
    public class HospitalPlanController : ControllerBase
    {
        private readonly ILogger<HospitalPlanController> _logger;
        private readonly IHospitalPlanService _HospitalPlanService;

        public HospitalPlanController(ILogger<HospitalPlanController> logger, IHospitalPlanService HospitalPlanService)
        {
            _logger = logger;
            _HospitalPlanService = HospitalPlanService;
        }

        [Authorize]
        [HttpGet("GetAllHospitalPlan")]
        public async Task<IActionResult> GetAllHospitalPlan()
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

                return Ok(await _HospitalPlanService.GetAllHospitalPlan(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllHospitalPlan() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllHospitalPlan() ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }
    }
}
