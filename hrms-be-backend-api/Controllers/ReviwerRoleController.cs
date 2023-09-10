using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hrms_be_backend_api.OnBoardingModuleController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviwerRoleController : ControllerBase
    {
        private readonly ILogger<ReviwerRoleController> _logger;
        private readonly IReviwerRoleService _ReviwerRoleService;

        public ReviwerRoleController(ILogger<ReviwerRoleController> logger, IReviwerRoleService ReviwerRoleService)
        {
            _logger = logger;
            _ReviwerRoleService = ReviwerRoleService;
        }


        [Authorize]
        [HttpGet("GetReviwerRoleById")]
        public async Task<IActionResult> GetReviwerRolebyId(long ReviwerRoleID)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _ReviwerRoleService.GetReviwerRolebyId(ReviwerRoleID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetReviwerRolebyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetReviwerRolebyId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [Authorize]
        [HttpGet("GetReviwerRolebyCompanyId")]
        public async Task<IActionResult> GetReviwerRolebyCompanyId(long CompanyID)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _ReviwerRoleService.GetReviwerRolebyCompanyId(CompanyID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetReviwerRolebyCompanyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetReviwerRolebyCompanyId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }
    }
}
