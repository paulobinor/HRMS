using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
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
        [Route("/SubmitResignationClearance")]
        public async Task<IActionResult> SubmitResignationClearance(ResignationClearanceVM request)
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

        [HttpPost]
        [Route("/UploadItemsReturnedToDepartmant")]
        public async Task<IActionResult> UploadFile_(IFormFile file)
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

                return Ok(await _resignationClearanceService.UploadItemsReturnedToDepartmant(file));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : UploadFile_ ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : UploadFile_ ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("GetResignationClearanceByID/{ID}")]
        public async Task<IActionResult> GetResignationClearanceByID(long ID)
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

                return Ok(await _resignationClearanceService.GetResignationClearanceByID(ID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetResignationClearanceByID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetResignationClearanceByID ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }
        [HttpGet]
        [Route("GetResignationByUserID/{userID}")]
        public async Task<IActionResult> GetResignationClearanceByUserID(long userID)
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

                return Ok(await _resignationClearanceService.GetResignationClearanceByUserID(userID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetResignationClearanceByUserID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetResignationClearanceByUserID ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }
        [HttpGet]
        [Route("GetPendingResignationClearanceByUserID/{userID}")]
        public async Task<IActionResult> GetPendingResignationClearanceByUserID(long userID)
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

                return Ok(await _resignationClearanceService.GetPendingResignationClearanceByUserID(requester, userID));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetPendingResignationClearanceByUserID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetPendingResignationClearanceByUserID ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [HttpPost]
        [Route("ApprovePendingResignation")]
        public async Task<IActionResult> ApprovePendingResignationClearance([FromBody] ApproveResignationClearanceDTO request)
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

                return Ok(await _resignationClearanceService.ApprovePendingResignationClearance(request, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : ApprovePendingResignationClearance ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : ApprovePendingResignationClearance ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [HttpPost]
        [Route("DisapprovePendingResignation")]
        public async Task<IActionResult> DisapprovePendingResignationClearance([FromBody] DisapprovePendingResignationClearanceDTO request)
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

                return Ok(await _resignationClearanceService.DisapprovePendingResignationClearance(request, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DisapprovePendingResignationClearance ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DisapprovePendingResignationClearance ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }


    }
}
