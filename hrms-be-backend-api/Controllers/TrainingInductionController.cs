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
    public class TrainingInductionController : BaseController
    {
        private readonly ILogger<TrainingInductionController> _logger;
        private readonly ITrainingInductionService _trainingInductionService;

        public TrainingInductionController(ILogger<TrainingInductionController> logger, ITrainingInductionService trainingInductionService)
        {
            _logger = logger;
            _trainingInductionService = trainingInductionService;
        }
        [HttpPost("CreateInductionPlan")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        [Authorize]
        public async Task<IActionResult> CreateTrainingInduction([FromBody] TrainingInductionCreate CreateDto)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _trainingInductionService.CreateTrainingInduction(CreateDto, accessToken, RemoteIpAddress));

        }

        [HttpPost("UpdateTrainingInduction")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        [Authorize]
        public async Task<IActionResult> UpdateTrainingInduction([FromBody] TrainingInductionUpdate UpdateDto)
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

                return this.CustomResponse(await _trainingInductionService.UpdateTrainingInduction(UpdateDto, accessToken, RemoteIpAddress));

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : UpdateTrainingInduction ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : UpdateTrainingInduction ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpPost("DeleteTrainingInduction")]
        public async Task<IActionResult> DeleteTrainingInduction([FromBody] TrainingInductionDelete DeleteDto)
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

                return this.CustomResponse(await _trainingInductionService.DeleteTrainingInduction(DeleteDto, accessToken, RemoteIpAddress));

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DeleteTrainingInduction ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DeleteTrainingInduction ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("ApproveTrainingInduction")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        [Authorize]
        public async Task<IActionResult> ApproveTrainingInduction([FromBody] long TrainingInductionID)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _trainingInductionService.ApproveTrainingInduction(TrainingInductionID, accessToken, RemoteIpAddress));

        }

        [HttpPost("DisaproveTrainingInduction")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        [Authorize]
        public async Task<IActionResult> DisaproveTrainingInduction([FromBody] TrainingInductionDisapproved payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return this.CustomResponse(await _trainingInductionService.DisaproveTrainingInduction(payload, accessToken, RemoteIpAddress));

        }

        [Authorize]
        [HttpGet("GetAllTrainingInduction")]
        public async Task<IActionResult> GetAllTrainingInduction()
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
                return this.CustomResponse(await _trainingInductionService.GetAllTrainingInduction(accessToken, RemoteIpAddress));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllTrainingInduction ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllTrainingInduction ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetAllActiveTrainingInduction")]
        public async Task<IActionResult> GetAllActiveTrainingInduction()
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
                return this.CustomResponse(await _trainingInductionService.GetAllActiveTrainingInduction(accessToken, RemoteIpAddress));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllActiveTrainingInduction ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllActiveTrainingInduction ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetTrainingInductionById")]
        public async Task<IActionResult> GetTrainingInductionById(long TrainingInductionID)
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
                return this.CustomResponse(await _trainingInductionService.GetTrainingInductionById(TrainingInductionID, accessToken, RemoteIpAddress));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetTrainingInductionbyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetTrainingInductionId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [Authorize]
        [HttpGet("GetTrainingInductionbyCompanyId")]
        public async Task<IActionResult> GetTrainingInductionbyCompanyId(long CompanyID)
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
                return this.CustomResponse(await _trainingInductionService.GetTrainingInductionbyCompanyId(CompanyID, accessToken, RemoteIpAddress));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetTrainingInductionbyCompanyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetTrainingInductionbyCompanyId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [Authorize]
        [HttpGet("GetTrainingInductionPendingApproval")]
        public async Task<IActionResult> GetTrainingInductionPendingApproval()
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
                return this.CustomResponse(await _trainingInductionService.GetTrainingInductionPendingApproval(accessToken, RemoteIpAddress));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetTrainingInductionPendingApproval ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetTrainingInductionPendingApproval ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }



    }
}
