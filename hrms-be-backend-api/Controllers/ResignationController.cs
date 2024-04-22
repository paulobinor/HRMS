using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_business.ILogic;
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
        [Route("SubmitResignation")]
        [Authorize]
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

                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
                accessToken = accessToken.ToString().Replace("bearer", "").Trim();

                return Ok(await _resignationService.SubmitResignation(request, accessToken, RemoteIpAddress));
            }
            catch (Exception ex) { }

        }

        [HttpPost]
        [Route("UpdateResignation")]
        [Authorize]
        public async Task<IActionResult> UpdateResignation([FromBody] UpdateResignationDTO updateDTO)
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _resignationService.UpdateResignation(updateDTO, accessToken, RemoteIpAddress));


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

                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
                accessToken = accessToken.ToString().Replace("bearer", "").Trim();

                return Ok(await _resignationService.UploadLetter(letter, accessToken, RemoteIpAddress));
            }
            catch (Exception ex) { }
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

                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
                accessToken = accessToken.ToString().Replace("bearer", "").Trim();

                return Ok(await _resignationService.GetResignationByID(resignationID, accessToken, RemoteIpAddress));
            }
            catch (Exception ex) { }

        }


        [HttpGet]
        [Route("GetResignationByEmployeeID/{EmployeeID}")]
        [Authorize]
        public async Task<IActionResult> GetResignationByEmployeeID(long EmployeeID)
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

                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
                accessToken = accessToken.ToString().Replace("bearer", "").Trim();

                return Ok(await _resignationService.GetResignationByEmployeeID(EmployeeID, accessToken, RemoteIpAddress));
            }
            catch (Exception ex) { }

        }

        //[Authorize]
        [HttpGet]
        [Route("GetResignationByCompanyID/{companyId}")]
        [Authorize]
        public async Task<IActionResult> GetResignationByCompanyID(long companyId, [FromQuery] PaginationFilter filter)
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _resignationService.GetResignationByCompanyID(filter, companyId, accessToken, RemoteIpAddress));

            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError($"Exception Occured: ControllerMethod : GetResignationByCompanyID ==> {ex.Message}");
            //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
            //    response.ResponseMessage = $"Exception Occured: ControllerMethod : GetResignationByCompanyID ==> {ex.Message}";
            //    response.Data = null;
            //    return Ok(response);
            //}

        }

        //[HttpGet]
        //[Route("GetResignationByCompanyID/{companyId}/{isApproved}")]
        //[Authorize]
        //public async Task<IActionResult> GetAllResignations()
        //{

        //    var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
        //    var identity = HttpContext.User.Identity as ClaimsIdentity;
        //    IEnumerable<Claim> claim = identity.Claims;
        //    var accessToken = Request.Headers["Authorization"];
        //    accessToken = accessToken.ToString().Replace("bearer", "").Trim();

        //    return Ok(await _resignationService.GetAllResignations(accessToken, RemoteIpAddress));


        //}


        //[Authorize]
        //[HttpPost]
        //[Route("DeleteResignation")]
        //[Authorize]
        //public async Task<IActionResult> DeleteResignation([FromBody] DeleteResignationDTO request)
        //{

        //        var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //        var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
        //        var identity = HttpContext.User.Identity as ClaimsIdentity;
        //        IEnumerable<Claim> claim = identity.Claims;
        //        var accessToken = Request.Headers["Authorization"];
        //        accessToken = accessToken.ToString().Replace("bearer", "").Trim();

        //        return Ok(await _resignationService.DeleteResignation(request, accessToken, RemoteIpAddress));

        //}

        [HttpGet]
        [Route("GetPendingResignationByEmployeeID/{employeeID}")]
        [Authorize]
        public async Task<IActionResult> GetPendingResignationByEmployeeID(long employeeID)
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _resignationService.GetPendingResignationByEmployeeID(employeeID, accessToken, RemoteIpAddress));

            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError($"Exception Occured: ControllerMethod : GetPendingResignationByUserID ==> {ex.Message}");
            //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
            //    response.ResponseMessage = $"Exception Occured: ControllerMethod : GetPendingResignationByUserID ==> {ex.Message}";
            //    response.Data = null;
            //    return Ok(response);
            //}

        }

        [Authorize]
        [HttpPost]
        [Route("ApprovePendingResignation")]
        [Authorize]
        public async Task<IActionResult> ApprovePendingResignation([FromBody] ApprovePendingResignationDTO request)
        {
            var response = new BaseResponse();
            //try
            //{
            var requester = new RequesterInfo
            {
                Username = this.User.Claims.ToList()[2].Value,
                UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                Port = Request.HttpContext.Connection.RemotePort.ToString()
            };

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _resignationService.ApprovePendingResignation(request, accessToken, RemoteIpAddress));


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

                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
                accessToken = accessToken.ToString().Replace("bearer", "").Trim();

                return Ok(await _resignationService.DisapprovePendingResignation(request, accessToken, RemoteIpAddress));
            }
            catch (Exception ex) { }
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
