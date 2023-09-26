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
    public class EmploymentStatusController : ControllerBase
    {
        private readonly ILogger<EmploymentStatusController> _logger;
        private readonly IEmploymentStatusService _EmploymentStatusService;

        public EmploymentStatusController(ILogger<EmploymentStatusController> logger, IEmploymentStatusService EmploymentStatusService)
        {
            _logger = logger;
            _EmploymentStatusService = EmploymentStatusService;
        }

        [HttpPost("CreateEmpLoymentStatus")]
        [Authorize]
        public async Task<IActionResult> CreateEmpLoymentStatus([FromBody] CreateEmploymentStatusDTO EmpLoymentStatusDto)
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

                return Ok(await _EmploymentStatusService.CreateEmploymentStatus(EmpLoymentStatusDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : CreateEmpLoymentStatus ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : CreateEmpLoymentStatus ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("CreateEmpLoymentStatusBulkUpload")]
        [Authorize]
        public async Task<IActionResult> CreateEmpLoymentStatusBulkUpload(IFormFile payload)
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

                return Ok(await _EmploymentStatusService.CreateEmploymentStatusBulkUpload(payload, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : CreateEmployeeTypeBulkUpload ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : CreateEmployeeTypeBulkUpload ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("UpdateEmpLoymentStatus")]
        [Authorize]
        public async Task<IActionResult> UpdateEmpLoymentStatus([FromBody] UpdateEmploymentStatusDTO updateDto)
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

                return Ok(await _EmploymentStatusService.UpdateEmploymentStatus(updateDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : UpdateEmpLoymentStatus ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : UpdateEmpLoymentStatus ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("DeleteEmpLoymentStatus")]
        [Authorize]
        public async Task<IActionResult> DeleteEmpLoymentStatus([FromBody] DeleteEmploymentStatusDTO deleteDto)
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

                return Ok(await _EmploymentStatusService.DeleteEmploymentStatus(deleteDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DeleteEmpLoymentStatus ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DeleteEmpLoymentStatus ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetAllActiveEmpLoymentStatus")]
        public async Task<IActionResult> GetAllActiveEmpLoymentStatus()
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

                return Ok(await _EmploymentStatusService.GetAllActiveEmploymentStatus(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllActiveEmpLoymentStatus ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllActiveEmpLoymentStatus ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [Authorize]
        [HttpGet("GetAllEmpLoymentStatus")]
        public async Task<IActionResult> GetAllEmpLoymentStatus()
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

                return Ok(await _EmploymentStatusService.GetAllEmploymentStatus(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllEmpLoymentStatus ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllEmpLoymentStatus ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetEmpLoymentStatusbyId")]
        public async Task<IActionResult> GetEmpLoymentStatusbyId(long EmploymentStatusID)
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

                return Ok(await _EmploymentStatusService.GetEmploymentStatusbyId(EmploymentStatusID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetEmpLoymentStatusbyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetEmpLoymentStatusbyId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [Authorize]
        [HttpGet("GetEmpLoymentStatusbyCompanyId")]
        public async Task<IActionResult> GetEmpLoymentStatusbyCompanyId(long CompanyID)
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

                return Ok(await _EmploymentStatusService.GetEmpLoymentStatusbyCompanyId(CompanyID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetEmpLoymentStatusbyCompanyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetEmpLoymentStatusbyCompanyId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

    }
}
