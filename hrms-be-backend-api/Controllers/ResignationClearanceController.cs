using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace hrms_be_backend_api.ExitModuleController.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResignationClearanceController : ControllerBase
    {
        private readonly ILogger<ResignationClearanceController> _logger;
        private readonly IResignationClearanceService _resignationClearanceService;

        public ResignationClearanceController(ILogger<ResignationClearanceController> logger, IResignationClearanceService resignationClearanceService)
        {
            _logger = logger;
            _resignationClearanceService = resignationClearanceService;
        }

        [HttpPost]

        public async Task<IActionResult> SubmitResignationClearance(ResignationClearanceVM request)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    //Username = this.User.Claims.ToList()[2].Value,
                    //UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    //RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    //IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    //Port = Request.HttpContext.Connection.RemotePort.ToString()
                };

                return Ok(await _resignationClearanceService.SubmitResignationClearance(requester, request));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : SubmitResignationClearance() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : SubmitResignationClearance() ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }
    }
}
