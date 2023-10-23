using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
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
    [Authorize]
    public class JobDescriptionController : BaseController
    {
        private readonly ILogger<JobDescriptionController> _logger;
        private readonly IJobDescriptionService _jobDescriptionService;

        public JobDescriptionController(ILogger<JobDescriptionController> logger, IJobDescriptionService jobDescriptionService)
        {
            _logger = logger;
            _jobDescriptionService = jobDescriptionService;
        }

        [HttpPost("CreateJobDescription")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> CreateJobDescription(CreateJobDescriptionDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _jobDescriptionService.CreateJobDescription(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("UpdateJobDescription")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> UpdateJobDescription(UpdateJobDescriptionDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _jobDescriptionService.UpdateJobDescription(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpPost("DeleteJobDescription")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        public async Task<IActionResult> DeleteJobDescription(DeleteJobDescriptionDto payload)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return this.CustomResponse(await _jobDescriptionService.DeleteJobDescription(payload, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetJobDescriptions")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<CompanyVm>>), 200)]
        public async Task<IActionResult> GetJobDescriptions([FromQuery] PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _jobDescriptionService.GetJobDescriptions(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetJobDescriptionsDeleted")]
        [ProducesResponseType(typeof(PagedExcutedResult<IEnumerable<CompanyVm>>), 200)]
        public async Task<IActionResult> GetJobDescriptionsDeleted([FromQuery] PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _jobDescriptionService.GetJobDescriptionsDeleted(filter, route, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetJobDescriptionById")]
        [ProducesResponseType(typeof(ExecutedResult<JobDescriptionVm>), 200)]
        public async Task<IActionResult> GetJobDescriptionById([FromQuery] long JobDescriptionId)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _jobDescriptionService.GetJobDescriptionById(JobDescriptionId, accessToken, claim, RemoteIpAddress, RemotePort));
        }
        [HttpGet("GetJobDescriptionByName")]
        [ProducesResponseType(typeof(ExecutedResult<JobDescriptionVm>), 200)]
        public async Task<IActionResult> GetJobDescriptionByName([FromQuery] string JobDescriptionName)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _jobDescriptionService.GetJobDescriptionByName(JobDescriptionName, accessToken, claim, RemoteIpAddress, RemotePort));
        }
    }
}
