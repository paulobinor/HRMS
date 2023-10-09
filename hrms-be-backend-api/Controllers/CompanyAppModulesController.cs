using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static hrms_be_backend_business.Logic.CompanyAppModuleService;

namespace hrms_be_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyAppModulesController : ControllerBase
    {
        private readonly ILogger<CompanyAppModulesController> _logger;
        private readonly ICompanyAppModuleService _companyAppModuleService;

        public CompanyAppModulesController(ILogger<CompanyAppModulesController> logger, ICompanyAppModuleService companyAppModuleService)
        {
            _logger = logger;
            _companyAppModuleService = companyAppModuleService;
        }

        [HttpGet("GetAllAppModules")]
        public async Task<IActionResult> GetAllAppModules()
        {
            var response = new BaseResponse();
            try
            {

                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _companyAppModuleService.GetAllAppModules(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllAppModules ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet("GetCompanyAppModuleCount")]
        public async Task<IActionResult> GetCompanyAppModuleCount()
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _companyAppModuleService.GetCompanyAppModuleCount(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetCompanyAppModuleCount ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet("GetCompanyAppModuleByCompanyID/{companyID}")]
        public async Task<IActionResult> GetCompanyAppModuleByCompanyID(long companyID)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _companyAppModuleService.GetCompanyAppModuleByCompanyID(companyID,requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetCompanyAppModuleByCompanyID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet("GetCompanyAppModuleBySatus/{status}")]
        public async Task<IActionResult> GetCompanyAppModuleByCompanyID(GetByStatus status)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _companyAppModuleService.GetCompanyAppModuleStatus(status, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetCompanyAppModuleByCompanyID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }


        [HttpGet("GetPendingCompanyAppModule")]
        public async Task<IActionResult> GetPendingCompanyAppModule()
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _companyAppModuleService.GetPendingCompanyAppModule(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetPendingCompanyAppModule ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("CreateCompanyAppModule")]
        public async Task<IActionResult> CreateCompanyAppModule(CreateCompanyAppModuleDTO request)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _companyAppModuleService.CreateCompanyAppModule(request, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : CreateCompanyAppModule ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet("ApproveCompanyAppModule/{companyAppModuleID}")]
        public async Task<IActionResult> ApproveCompanyAppModule(long companyAppModuleID)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _companyAppModuleService.ApproveCompanyAppModule(companyAppModuleID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : ApproveCompanyAppModule ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet("DisapproveCompanyAppModule/{companyAppModuleID}")]
        public async Task<IActionResult> DisapproveCompanyAppModule(long companyAppModuleID)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _companyAppModuleService.DisapproveCompanyAppModule(companyAppModuleID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DisapproveCompanyAppModule ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet("DeleteCompanyAppModule/{companyAppModuleID}")]
        public async Task<IActionResult> DeleteCompanyAppModule(long companyAppModuleID)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _companyAppModuleService.DeleteCompanyAppModule(companyAppModuleID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DeleteCompanyAppModule ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }
        [HttpGet("CompanyAppModuleActivationSwitch/{companyAppModuleID}")]
        public async Task<IActionResult> CompanyAppModuleActivationSwitch(long companyAppModuleID)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _companyAppModuleService.CompanyAppModuleActivationSwitch(companyAppModuleID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : CompanyAppModuleActivationSwitch ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }
    }
}
