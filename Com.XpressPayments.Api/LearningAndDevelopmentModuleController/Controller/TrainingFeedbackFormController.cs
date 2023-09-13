using Com.XpressPayments.Bussiness.LearningAndDevelopmentModuleService.Service.ILogic;
using Com.XpressPayments.Bussiness.LearningAndDevelopmentModuleService.Service.Logic;
using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Com.XpressPayments.Api.LearningAndDevelopmentModuleController.Controller
{
    public class TrainingFeedbackFormController : ControllerBase
    {
        private readonly ILogger<TrainingFeedbackFormController> _logger;
        private readonly ITrainingFeedbackFormService _trainingFeedbackFormService;

        public TrainingFeedbackFormController(ILogger<TrainingFeedbackFormController> logger, ITrainingFeedbackFormService trainingFeedbackFormService)
        {
            _logger = logger;
            _trainingFeedbackFormService = trainingFeedbackFormService;
        }
        [HttpPost("CreateSupervisorTrainingFeedbackForm")]
        [Authorize]
        public async Task<IActionResult> CreateSupervisorTrainingFeedbackForm([FromBody] SupervisorTrainingFeedbackFormCreate CreateDto)
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

            return Ok(await _trainingFeedbackFormService.CreateSupervisorTrainingFeedbackForm(CreateDto, requester));
        }

        [HttpPost("CreateTraineeTrainingFeedbackForm")]
        [Authorize]
        public async Task<IActionResult> CreateTraineeTrainingFeedbackForm([FromBody] TraineeTrainingFeedbackFormCreate CreateDto)
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

            return Ok(await _trainingFeedbackFormService.CreateTraineeTrainingFeedbackForm(CreateDto, requester));
        }
    }
}
