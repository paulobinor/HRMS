using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_business.Logic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace hrms_be_backend_api.ExitModuleController.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResignationInterviewController : ControllerBase
    {
        private readonly ILogger<ResignationInterviewController> _logger;
        private readonly IResignationInterviewService _resignationInterviewService;

        public ResignationInterviewController(ILogger<ResignationInterviewController> logger, IResignationInterviewService resignationInterviewService)
        {
            _logger = logger;
            _resignationInterviewService = resignationInterviewService;
        }

        [HttpPost("SubmitResignationInterview")]
        [ProducesResponseType(typeof(ExecutedResult<string>), 200)]
        [Authorize]
        public async Task<IActionResult> SubmitResignationInterview(ResignationInterviewVM request)
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
                    Port =  Request.HttpContext.Connection.RemotePort.ToString()
                };
            
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
                accessToken = accessToken.ToString().Replace("bearer", "").Trim();

                return Ok(await _resignationInterviewService.SubmitResignationInterview(request, accessToken, RemoteIpAddress));
          
                return Ok(await _resignationInterviewService.GetInterviewScaleDetails(requester));
        }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetInterviewScaleDetails() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetInterviewScaleDetails() ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }


        [HttpGet]
        [Route("GetResignationInterview/{ResignationInterviewId}")]
        [Authorize]
        public async Task<IActionResult> GetResignationInterview(long ResignationInterviewId)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _resignationInterviewService.GetResignationInterviewById(ResignationInterviewId, accessToken, RemoteIpAddress));
        
        }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetInterviewDetails() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetInterviewDetails() ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("GetResignationInterviewDetails/{InterviewID}")]
        [Authorize]
        public async Task<IActionResult> GetResignationInterviewDetails(long InterviewID)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _resignationInterviewService.GetResignationInterviewDetails(InterviewID, accessToken, RemoteIpAddress));
            
        } 
        [HttpGet]
        [Route("GetAllResignationInterviewsByCompany/{CompanyID}")]
        [Authorize]
        public async Task<IActionResult> GetAllResignationInterviewsByCompany(long CompanyID, [FromQuery] PaginationFilter filter)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _resignationInterviewService.GetAllResignationInterviewsByCompany(filter,CompanyID, accessToken, RemoteIpAddress));
            
                return Ok(await _resignationInterviewService.ApprovePendingResignationInterview(request, requester));
        }

        [HttpGet]
        [Route("GetResignationInterviewByEmployeeID/{EmployeeID}")]
        [Authorize]
        public async Task<IActionResult> GetResignationInterviewByEmployeeID(long EmployeeID)
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _resignationInterviewService.GetResignationInterviewByEmployeeID(EmployeeID, accessToken, RemoteIpAddress));


        }

        //[HttpGet]
        //public async Task<IActionResult> GetInterviewScaleDetails()
        //{

        //    var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
        //    var identity = HttpContext.User.Identity as ClaimsIdentity;
        //    IEnumerable<Claim> claim = identity.Claims;
        //    var accessToken = Request.Headers["Authorization"];
        //    accessToken = accessToken.ToString().Replace("bearer", "").Trim();

        //    return Ok(await _resignationInterviewService.GetInterviewScaleDetails(accessToken, RemoteIpAddress));

        //}

        //[HttpPost]
        //[Route("ApprovePendingResignationInterview")]
        //public async Task<IActionResult> ApprovePendingResignationInterview([FromBody] ApproveResignationInterviewDTO request)
        //{
        //    var response = new BaseResponse();
        //    try
        //    {
        //        var requester = new RequesterInfo
        //        {
        //            Username = this.User.Claims.ToList()[2].Value,
        //            UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
        //            RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
        //            IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
        //            Port =  Request.HttpContext.Connection.RemotePort.ToString()
        //        };

        //        return Ok(await _resignationInterviewService.ApprovePendingResignationInterview(request, requester));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: ControllerMethod : ApprovePendingResignationInterview ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured: ControllerMethod : ApprovePendingResignationInterview ==> {ex.Message}";
        //        response.Data = null;
        //        return Ok(response);
        //    }

        //}

        //[HttpPost]
        //[Route("DisapprovePendingResignationInterview")]
        //public async Task<IActionResult> DisapprovePendingResignationInterview([FromBody] DisapproveResignationInterviewDTO request)
        //{
        //    var response = new BaseResponse();
        //    try
        //    {
        //        var requester = new RequesterInfo
        //        {
        //            Username = this.User.Claims.ToList()[2].Value,
        //            UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
        //            RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
        //            IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
        //            Port =  Request.HttpContext.Connection.RemotePort.ToString()
        //        };

        //        return Ok(await _resignationInterviewService.DisapprovePendingResignationInterview(request, requester));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: ControllerMethod : DisapprovePendingResignationInterview ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured: ControllerMethod : DisapprovePendingResignationInterview ==> {ex.Message}";
        //        response.Data = null;
        //        return Ok(response);
        //    }

        //}

        //[HttpGet]
        //[Route("GetAllApprovedResignationInterviewSwitch/{UserID}/{isApproved}")]
        //public async Task<IActionResult> GetAllApprovedResignationInterview(long UserID, bool isApproved)
        //{
        //    var response = new BaseResponse();
        //    try
        //    {
        //        var requester = new RequesterInfo
        //        {
        //            Username = this.User.Claims.ToList()[2].Value,
        //            UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
        //            RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
        //            IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
        //            Port =  Request.HttpContext.Connection.RemotePort.ToString()
        //        };

        //        return Ok(await _resignationInterviewService.GetAllApprovedResignationInterview(UserID, isApproved, requester));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: ControllerMethod : GetAllApprovedResignationInterview() ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllApprovedResignationInterview() ==> {ex.Message}";
        //        response.Data = null;
        //        return Ok(response);
        //    }
        //}
    }
}
