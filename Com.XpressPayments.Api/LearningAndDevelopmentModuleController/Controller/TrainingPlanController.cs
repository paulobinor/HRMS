
using Com.XpressPayments.Bussiness.LearningAndDevelopmentModuleService.Service.ILogic;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs;
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
    public class TrainingPlanController : ControllerBase
    {
        private readonly ILogger<TrainingPlanController> _logger;
        private readonly ITrainingPlanService _trainingPlanService;

        public TrainingPlanController(ILogger<TrainingPlanController> logger, ITrainingPlanService trainingPlanService)
        {
            _logger = logger;
            _trainingPlanService = trainingPlanService;
        }
        [HttpPost("CreateTrainingPlan")]
        [Authorize]
        public async Task<IActionResult> CreateTrainingPlan([FromBody] TrainingPlanCreate CreateDto)
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

            return Ok(await _trainingPlanService.CreateTrainingPlan(CreateDto, requester));
        }

        [HttpPost("UpdateTrainingPlan")]
        [Authorize]
        public async Task<IActionResult> UpdateTrainingPlan([FromBody] TrainingPlanUpdate UpdateDto)
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

                return Ok(await _trainingPlanService.UpdateTrainingPlan(UpdateDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : UpdateTrainingPlan ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : UpdateTrainingPlan ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpPost("DeleteTrainingPlan")]
        public async Task<IActionResult> DeleteTrainingPlan([FromBody] TrainingPlanDelete DeleteDto)
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

                return Ok(await _trainingPlanService.DeleteTrainingPlan(DeleteDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DeleteTrainingPlan ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DeleteTrainingPlan ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("ApproveTrainingPlan")]
        [Authorize]
        public async Task<IActionResult> ApproveTrainingPlan([FromBody] long TrainingPlanID)
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

            return Ok(await _trainingPlanService.ApproveTrainingPlan(TrainingPlanID, requester));
        }

        [HttpPost("DisaproveTrainingPlan")]
        [Authorize]
        public async Task<IActionResult> DisaproveTrainingPlan([FromBody] TrainingPlanDisapproved payload)
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

            return Ok(await _trainingPlanService.DisaproveTrainingPlan(payload, requester));
        }

        [Authorize]
        [HttpGet("GetAllTrainingPlan")]
        public async Task<IActionResult> GetAllTrainingPlan()
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

                return Ok(await _trainingPlanService.GetAllTrainingPlan(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllTrainingPlan ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllTrainingPlan ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        } 

        [Authorize]
        [HttpGet("GetAllTrainingPlanByUser")]
        public async Task<IActionResult> GetAllTrainingPlanByUser(long UserId)
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

                return Ok(await _trainingPlanService.GetAllTrainingPlanByUserId(UserId,requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllTrainingPlan ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllTrainingPlanByUser ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetAllActiveTrainingPlan")]
        public async Task<IActionResult> GetAllActiveTrainingPlan()
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

                return Ok(await _trainingPlanService.GetAllActiveTrainingPlan(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllTrainingPlan ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllTrainingPlan ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetTrainingPlanById")]
        public async Task<IActionResult> GetTrainingPlanById(long TrainingPlanID)
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

                return Ok(await _trainingPlanService.GetTrainingPlanById(TrainingPlanID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetTrainingPlanbyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetTrainingPlanId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [Authorize]
        [HttpGet("GetTrainingPlanbyCompanyId")]
        public async Task<IActionResult> GetTrainingPlanbyCompanyId( long CompanyID)
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

                return Ok(await _trainingPlanService.GetTrainingPlanbyCompanyId(CompanyID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetTrainingPlanbyCompanyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetTrainingPlanbyCompanyId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [Authorize]
        [HttpGet("GetTrainingPlanPendingApproval")]
        public async Task<IActionResult> GetTrainingPlanPendingApproval()
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

                return Ok(await _trainingPlanService.GetTrainingPlanPendingApproval(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetTrainingPlanPendingApproval ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetTrainingPlanPendingApproval ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }



        [HttpPost("ScheuledTrainingPlan")]
        [Authorize]
        public async Task<IActionResult> ScheduledTrainingPlan([FromBody] TrainingPlanSchedule ScheduleDto)
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

                return Ok(await _trainingPlanService.ScheduleTrainingPlan(ScheduleDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : ScheduledTrainingPlan ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : ScheduledTrainingPlan ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }



    }
}
