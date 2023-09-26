using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hrms_be_backend_api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class JobDescriptionController : ControllerBase
    {
        private readonly ILogger<JobDescriptionController> _logger;
        private readonly IJobDescriptionService _jobDescriptionService;

        public JobDescriptionController(ILogger<JobDescriptionController> logger, IJobDescriptionService jobDescriptionService)
        {
            _logger = logger;
            _jobDescriptionService = jobDescriptionService;
        }

        [HttpPost("CreateJobDescription")]
        [Authorize]
        public async Task<IActionResult> CreateJobDescription([FromBody] CreateJobDescriptionDTO CreateDto)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _jobDescriptionService.CreateJobDescription(CreateDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : CreateJobDescription ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : CreateJobDescription ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }


        [HttpPost("CreateJobDescriptionBulkUpload")]
        [Authorize]
        public async Task<IActionResult> CreateJobDescriptionBulkUpload(IFormFile payload)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _jobDescriptionService.CreateJobDescriptionBulkUpload(payload, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : CreateJobDescriptionBulkUpload ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : CreateJobDescriptionBulkUpload ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("UpdateJobDescription")]
        [Authorize]
        public async Task<IActionResult> UpdateJobDescription([FromBody] UpdateJobDescriptionDTO updateDto)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _jobDescriptionService.UpdateJobDescription(updateDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : UpdateJobDescription ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : UpdateJobDescription ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("DeleteJobDescription")]
        [Authorize]
        public async Task<IActionResult> DeleteJobDescription([FromBody] DeletedJobDescriptionDTO deleteDto)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _jobDescriptionService.DeleteJobDescription(deleteDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DeleteJobDescription ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DeleteJobDescription ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetAllActiveJobDescription")]
        public async Task<IActionResult> GetAllActiveUnitHead()
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _jobDescriptionService.GetAllActiveJobDescription(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllActiveJobDescription ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllActiveJobDescription ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [Authorize]
        [HttpGet("GetAllJobDescription")]
        public async Task<IActionResult> GetAllJobDescription()
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _jobDescriptionService.GetAllJobDescription(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllJobDescription ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllJobDescription ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetJobDescriptionbyId")]
        public async Task<IActionResult> GetUnitHeadbyId(long UnitHeadID)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _jobDescriptionService.GetJobDescriptionById(UnitHeadID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetJobDescriptionById ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetJobDescriptionById ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }


        [Authorize]
        [HttpGet("GetJobDescriptionbyCompanyId")]
        public async Task<IActionResult> GetJobDescriptionbyCompanyId(long CompanyID)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _jobDescriptionService.GetJobDescriptionbyCompanyId(CompanyID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetJobDescriptionbyCompanyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetJobDescriptionbyCompanyId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

    }
}
