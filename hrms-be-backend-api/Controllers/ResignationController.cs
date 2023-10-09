using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hrms_be_backend_api.ExitModuleController.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResignationController : ControllerBase
    {
        private readonly ILogger<ResignationController> _logger;
        private readonly IResignationService _resignationService;

        public ResignationController(ILogger<ResignationController> logger, IResignationService resignationService)
        {
            _logger = logger;
            _resignationService = resignationService;
        }
        [HttpPost]
        public async Task<IActionResult> SubmitResignation(ResignationRequestVM request)
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

                return Ok(await _resignationService.SubmitResignation(requester, request));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : SubmitResignation ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : SubmitResignation ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }


        [HttpPost]
        [Route("UploadResignationLetter")]
        [Authorize]
        public async Task<IActionResult> UploadFile_(IFormFile letter)
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

                return Ok(await _resignationService.UploadLetter(letter));
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


        //[Authorize]
        [HttpGet]
        [Route("GetResignationByID/{resignationID}")]
        [Authorize]
        public async Task<IActionResult> GetResignationByID(long resignationID)
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

                return Ok(await _resignationService.GetResignationByID(resignationID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetResignationByID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetResignationByID ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }


        [HttpGet]
        [Route("GetResignationByUserID/{userID}")]
        [Authorize]
        public async Task<IActionResult> GetResignationByUserID(long userID)
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

                return Ok(await _resignationService.GetResignationByUserID(userID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetResignationByUserID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetResignationByUserID ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        //[Authorize]
        [HttpGet]
        [Route("GetResignationByCompanyID/{companyId}/{isApproved}")]
        [Authorize]
        public async Task<IActionResult> GetResignationByCompanyID(long companyId, bool isApproved)
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

                return Ok(await _resignationService.GetResignationByCompanyID(companyId, isApproved, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetResignationByCompanyID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetResignationByCompanyID ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }


        //[Authorize]
        [HttpPost]
        [Route("DeleteResignation")]
        [Authorize]
        public async Task<IActionResult> DeleteResignation([FromBody] DeleteResignationDTO request)
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

                return Ok(await _resignationService.DeleteResignation(request, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DeleteResignation ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DeleteResignation ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("GetPendingResignationByUserID/{userID}")]
        [Authorize]
        public async Task<IActionResult> GetPendingResignationByUserID(long userID)
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

                return Ok(await _resignationService.GetPendingResignationByUserID(requester, userID));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetPendingResignationByUserID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetPendingResignationByUserID ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        //[Authorize]
        [HttpPost]
        [Route("ApprovePendingResignation")]
        //[Authorize]
        public async Task<IActionResult> ApprovePendingResignation([FromBody] ApprovePendingResignationDTO request)
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

                return Ok(await _resignationService.ApprovePendingResignation(request, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : ApprovePendingResignation ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : ApprovePendingResignation ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [HttpPost]
        [Route("DisapprovePendingResignation")]
        [Authorize]
        public async Task<IActionResult> DisapprovePendingResignation([FromBody] DisapprovePendingResignation request)
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

                return Ok(await _resignationService.DisapprovePendingResignation(request, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DisapprovePendingResignation ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DisapprovePendingResignation ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [HttpPost]
        [Route("UpdateResignation")]
        [Authorize]
        public async Task<IActionResult> UpdateResignation([FromBody] UpdateResignationDTO updateDTO)
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

                return Ok(await _resignationService.UpdateResignation(updateDTO, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : UpdateResignation ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : UpdateResignation ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }
    }
}
