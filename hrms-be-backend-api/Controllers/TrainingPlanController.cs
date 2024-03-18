using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace hrms_be_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingPlanController : BaseController
    {
        private readonly ILogger<TrainingPlanController> _logger;
        private readonly ITrainingPlanService _trainingPlanService;

        public TrainingPlanController(ILogger<TrainingPlanController> logger, ITrainingPlanService trainingPlanService)
        {
            _logger = logger;
            _trainingPlanService = trainingPlanService;
        }
        [HttpPost("CreateTrainingPlan")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        [Authorize]
        public async Task<IActionResult> CreateTrainingPlan([FromBody] TrainingPlanCreate CreateDto)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _trainingPlanService.CreateTrainingPlan(CreateDto, accessToken, RemoteIpAddress));
        }

        [HttpPost("UpdateTrainingPlan")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        [Authorize]
        public async Task<IActionResult> UpdateTrainingPlan([FromBody] TrainingPlanUpdate UpdateDto)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
                accessToken = accessToken.ToString().Replace("bearer", "").Trim();

                return this.CustomResponse(await _trainingPlanService.UpdateTrainingPlan(UpdateDto, accessToken, RemoteIpAddress));
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
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        [HttpPost("DeleteTrainingPlan")]
        public async Task<IActionResult> DeleteTrainingPlan([FromBody] TrainingPlanDelete DeleteDto)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
                accessToken = accessToken.ToString().Replace("bearer", "").Trim();

                return this.CustomResponse(await _trainingPlanService.DeleteTrainingPlan(DeleteDto, accessToken, RemoteIpAddress));
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
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        [Authorize]
        public async Task<IActionResult> ApproveTrainingPlan([FromBody] TrainingPlanApproved payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _trainingPlanService.ApproveTrainingPlan(payload, accessToken, RemoteIpAddress));
        }

        [HttpPost("DisaproveTrainingPlan")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        [Authorize]
        public async Task<IActionResult> DisaproveTrainingPlan([FromBody] TrainingPlanDisapproved payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _trainingPlanService.DisapproveTrainingPlan(payload, accessToken, RemoteIpAddress));
        }

        [Authorize]
        [HttpGet("GetAllTrainingPlan")]
        public async Task<IActionResult> GetAllTrainingPlan()
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
                accessToken = accessToken.ToString().Replace("bearer", "").Trim();
                return this.CustomResponse(await _trainingPlanService.GetAllTrainingPlan(accessToken, RemoteIpAddress));
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
        public async Task<IActionResult> GetAllTrainingPlanByUser(long EmployeeId)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
                accessToken = accessToken.ToString().Replace("bearer", "").Trim();
                return this.CustomResponse(await _trainingPlanService.GetAllTrainingPlanByUserId(EmployeeId, accessToken, RemoteIpAddress));
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
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
                accessToken = accessToken.ToString().Replace("bearer", "").Trim();
                return this.CustomResponse(await _trainingPlanService.GetAllActiveTrainingPlan(accessToken, RemoteIpAddress));
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
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
                accessToken = accessToken.ToString().Replace("bearer", "").Trim();
                return this.CustomResponse(await _trainingPlanService.GetTrainingPlanById(TrainingPlanID, accessToken, RemoteIpAddress));
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
        public async Task<IActionResult> GetTrainingPlanbyCompanyId(long CompanyID)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
                accessToken = accessToken.ToString().Replace("bearer", "").Trim();
                return this.CustomResponse(await _trainingPlanService.GetTrainingPlanbyCompanyId(CompanyID, accessToken, RemoteIpAddress));
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
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
                accessToken = accessToken.ToString().Replace("bearer", "").Trim();
                return this.CustomResponse(await _trainingPlanService.GetTrainingPlanPendingApproval(accessToken, RemoteIpAddress));
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
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        [Authorize]
        public async Task<IActionResult> ScheduledTrainingPlan([FromBody] TrainingPlanSchedule ScheduleDto)
        {
            var response = new BaseResponse();
            try
            {
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
                accessToken = accessToken.ToString().Replace("bearer", "").Trim();
                return this.CustomResponse(await _trainingPlanService.ScheduleTrainingPlan(ScheduleDto, accessToken, RemoteIpAddress));
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
