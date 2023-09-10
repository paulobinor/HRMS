using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hrms_be_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstitutionsController : ControllerBase
    {
        private readonly ILogger<InstitutionsController> _logger;
        private readonly IInstitutionsService _InstitutionsService;

        public InstitutionsController(ILogger<InstitutionsController> logger, IInstitutionsService InstitutionsService)
        {
            _logger = logger;
            _InstitutionsService = InstitutionsService;
        }

        [Authorize]
        [HttpGet("GetAllInstitutions")]
        public async Task<IActionResult> GetAllInstitutions()
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

                return Ok(await _InstitutionsService.GetAllInstitutions(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllInstitutions() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllInstitutions() ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

    }
}
