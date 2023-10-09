using hrms_be_backend_business.ILogic;
using hrms_be_backend_business.Logic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static hrms_be_backend_business.Logic.CompanyAppModuleService;

namespace hrms_be_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentalModulesController : ControllerBase
    {
        private readonly ILogger<DepartmentalModulesController> _logger;
        private readonly IDepartmentalModulesService _departmentalModulesService;

        public DepartmentalModulesController(ILogger<DepartmentalModulesController> logger, IDepartmentalModulesService departmentalModulesService)
        {
            _logger = logger;
            _departmentalModulesService = departmentalModulesService;
        }

       

        [HttpGet("GetDepartmetalAppModuleCount")]
        public async Task<IActionResult> GetDepartmetalAppModuleCount()
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

                return Ok(await _departmentalModulesService.GetDepartmentalAppModuleCount(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetDepartmetalAppModuleCount ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet("GetDepartmentAppModuleBySatus/{status}")]
        public async Task<IActionResult> GetDepartmentAppModuleByStatus(GetByStatus status)
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

                return Ok(await _departmentalModulesService.GetDepartmentAppModuleStatus(status, requester));
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

        [HttpGet("GetDepartmentAppModuleByDepartmentID/{departmentID}")]
        public async Task<IActionResult> GetDepartmentAppModuleByDepartmentID(long departmentID)
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

                return Ok(await _departmentalModulesService.GetDepartmentalAppModuleByDepartmentID(departmentID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetDepartmentAppModuleByDepartmentID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }



        [HttpGet("GetPendingDepartmentalAppModule")]
        public async Task<IActionResult> GetPendingDepartmentalAppModule()
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

                return Ok(await _departmentalModulesService.GetPendingDepartmentalAppModule(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetPendingDepartmentalAppModule ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("CreateDepartmentalAppModule")]
        public async Task<IActionResult> CreateDepartmentalAppModule(CreateDepartmentalModuleDTO request)
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

                return Ok(await _departmentalModulesService.CreateDepartmentalAppModule(request, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : CreateDepartmentalAppModule ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet("ApproveDepartmentAppModule/{departmentAppModuleID}")]
        public async Task<IActionResult> ApproveDepartmentAppModule(long departmentAppModuleID)
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

                return Ok(await _departmentalModulesService.ApproveDepartmentalAppModule(departmentAppModuleID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : ApproveDepartmentAppModule ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet("DisapproveDepartmentalAppModule/{departmentAppModuleID}")]
        public async Task<IActionResult> DisapproveDepartmentalAppModule(long departmentAppModuleID)
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

                return Ok(await _departmentalModulesService.DisapproveDepartmentalAppModule(departmentAppModuleID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DisapproveDepartmentalAppModule ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet("DeleteDepartmentAppModule/{departmentAppModuleID}")]
        public async Task<IActionResult> DeleteDepartmentalAppModule(long departmentAppModuleID)
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

                return Ok(await _departmentalModulesService.DeleteDepartmentAppModule(departmentAppModuleID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DeleteDepartmentalAppModule ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }
        [HttpGet("DepartmentalAppModuleActivationSwitch/{departmentAppModuleID}")]
        public async Task<IActionResult> DepartmentalAppModuleActivationSwitch(long departmentAppModuleID)
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

                return Ok(await _departmentalModulesService.DepartmentAppModuleActivationSwitch(departmentAppModuleID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DepartmentalAppModuleActivationSwitch ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Anunexpected error occured";
                response.Data = null;
                return Ok(response);
            }
        }

    }
}
