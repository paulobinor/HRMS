using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace hrms_be_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    {
        private readonly ILogger<TrainingFeedbackFormController> _logger;
        private readonly ITrainingFeedbackFormService _trainingFeedbackFormService;

        public TrainingFeedbackFormController(ILogger<TrainingFeedbackFormController> logger, ITrainingFeedbackFormService trainingFeedbackFormService)
        {
            _logger = logger;
            _trainingFeedbackFormService = trainingFeedbackFormService;
        }
        [HttpPost("CreateSupervisorTrainingFeedbackForm")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        [Authorize]
        public async Task<IActionResult> CreateSupervisorTrainingFeedbackForm([FromBody] SupervisorTrainingFeedbackFormCreate CreateDto)
        {

        }

        [HttpPost("CreateTraineeTrainingFeedbackForm")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        [Authorize]
        public async Task<IActionResult> CreateTraineeTrainingFeedbackForm([FromBody] TraineeTrainingFeedbackFormCreate CreateDto)
        {

            return Ok(await _trainingFeedbackFormService.CreateTraineeTrainingFeedbackForm(CreateDto, requester));
        }
    }
}
