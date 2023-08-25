using Com.XpressPayments.Bussiness.LearningAndDevelopmentModuleService.Service.ILogic;
using Com.XpressPayments.Bussiness.LeaveModuleService.Service.ILogic;
using Com.XpressPayments.Bussiness.LeaveModuleService.Service.Logic;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs;
using Com.XpressPayments.Data.LeaveModuleDTO.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;

namespace Com.XpressPayments.Api.LearningAndDevelopmentModuleController.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingScheduleController : ControllerBase
    {
        private readonly ILogger<TrainingScheduleController> _logger;
        private readonly ITrainingScheduleService _trainingScheduleService;

        public TrainingScheduleController(ILogger<TrainingScheduleController> logger, ITrainingScheduleService trainingScheduleService)
        {
            _logger = logger;
            _trainingScheduleService = trainingScheduleService;
        }
        [HttpPost("CreateTrainingSchedule")]
        [Authorize]
        public async Task<IActionResult> CreateTrainingSchedule([FromBody] TrainingScheduleCreate CreateDto)
        {
            var response = new BaseResponse();
            var requester = new RequesterInfo
            {
                Username = this.User.Claims.ToList()[2].Value,
                UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                Port = Request.HttpContext.Connection.LocalPort.ToString()
            };

            return Ok(await _trainingScheduleService.CreateTrainingSchedule(CreateDto, requester));
        }

        [Authorize]
        [HttpGet("GetAllTrainingSchedule")]
        public async Task<IActionResult> GetAllTrainingSchedule()
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

                return Ok(await _trainingScheduleService.GetAllTrainingSchedule(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllTrainingSchedule ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllTrainingSchedule ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetTrainingSchedulebyId")]
        public async Task<IActionResult> GetTrainingSchedulebyId(long TrainingScheduleId, long CompanyId)
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

                return Ok(await _trainingScheduleService.GetTrainingScheduleById(TrainingScheduleId,  requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetTrainingSchedulebyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetTrainingSchedulebyId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }
        [HttpPost("ApproveTrainingSchedule")]
        [Authorize]
        public async Task<IActionResult> ApproveTrainingSchedule([FromBody] long TrainingScheduleID)
        {
            var response = new BaseResponse();
            var requester = new RequesterInfo
            {
                Username = this.User.Claims.ToList()[2].Value,
                UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                Port = Request.HttpContext.Connection.LocalPort.ToString()
            };

            return Ok(await _trainingScheduleService.ApproveTrainingSchedule(TrainingScheduleID, requester));
        }
        [HttpPost("DisaproveTrainingSchedule")]
        [Authorize]
        public async Task<IActionResult> DisaproveTrainingSchedule([FromBody] TrainingScheduleDisapproved payload)
        {
            var response = new BaseResponse();
            var requester = new RequesterInfo
            {
                Username = this.User.Claims.ToList()[2].Value,
                UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                Port = Request.HttpContext.Connection.LocalPort.ToString()
            };

            return Ok(await _trainingScheduleService.DisaproveTrainingSchedule(payload, requester));
        }

        [Authorize]
        [HttpGet("GetTrainingSchedulebyCompanyId")]
        public async Task<IActionResult> GetTrainingSchedulebyCompanyId( long CompanyID)
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

                return Ok(await _trainingScheduleService.GetTrainingSchedulebyCompanyId(CompanyID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetTrainingSchedulebyCompanyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetTrainingSchedulebyCompanyId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

    }
}
