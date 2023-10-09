using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hrms_be_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                Port =  Request.HttpContext.Connection.RemotePort.ToString()
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
                IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                Port =  Request.HttpContext.Connection.RemotePort.ToString()
            };

            return Ok(await _trainingFeedbackFormService.CreateTraineeTrainingFeedbackForm(CreateDto, requester));
        }
    }
}
