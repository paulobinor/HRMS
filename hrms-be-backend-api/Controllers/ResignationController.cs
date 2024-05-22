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
          
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
                accessToken = accessToken.ToString().Replace("bearer", "").Trim();

                return Ok(await _resignationService.SubmitResignation( request, accessToken, RemoteIpAddress));
       
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
           
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
                accessToken = accessToken.ToString().Replace("bearer", "").Trim();

                return Ok(await _resignationService.UploadLetter(letter, accessToken, RemoteIpAddress));
           
        }


        [HttpGet]
        [Route("GetResignationByID/{resignationID}")]
        [Authorize]
        public async Task<IActionResult> GetResignationByID(long resignationID)
        {
            
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
                accessToken = accessToken.ToString().Replace("bearer", "").Trim();

                return Ok(await _resignationService.GetResignationByID(resignationID, accessToken, RemoteIpAddress));
           

        }


        [HttpGet]
        [Route("GetResignationByEmployeeID/{EmployeeID}")]
        [Authorize]
        public async Task<IActionResult> GetResignationByEmployeeID(long EmployeeID)
        {
           
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _resignationService.GetResignationByEmployeeID(EmployeeID, accessToken, RemoteIpAddress));
            
           
        }

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
            

        }

        [HttpGet]
        [Route("GetPendingResignationByCompanyID/{companyID}")]
        [Authorize]
        public async Task<IActionResult> GetPendingResignationByCompanyID(long companyID)
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _resignationService.GetPendingResignationByCompanyID(companyID, accessToken, RemoteIpAddress));


        }


        [HttpPost]
        [Route("ApprovePendingResignation")]
        [Authorize]
        public async Task<IActionResult> ApprovePendingResignation([FromBody] ApprovePendingResignationDTO request)
        {
            
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
            
                var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claim = identity.Claims;
                var accessToken = Request.Headers["Authorization"];
                accessToken = accessToken.ToString().Replace("bearer", "").Trim();

                return Ok(await _resignationService.DisapprovePendingResignation(request, accessToken, RemoteIpAddress));
           

        }

        [HttpGet]
        [Route("GetReasonsForResignationByID/{ResignationID}")]
        [Authorize]
        public async Task<IActionResult> GetReasonsForResignationByID(long ResignationID)
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _resignationService.GetReasonsForResignationByID(ResignationID, accessToken, RemoteIpAddress));


        }
    }
}

