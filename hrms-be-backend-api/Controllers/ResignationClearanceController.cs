using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Security.Claims;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

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
        [Authorize]
        public async Task<IActionResult> SubmitResignationClearance(ResignationClearanceVM request)
        {
            
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _resignationClearanceService.SubmitResignationClearance(request, accessToken, RemoteIpAddress));
            
        }



        [HttpGet]
        [Route("GetResignationClearanceByID/{ID}")]
        [Authorize]
        public async Task<IActionResult> GetResignationClearanceByID(long ID)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _resignationClearanceService.GetResignationClearanceByID(ID, accessToken, RemoteIpAddress));
 

        }
        [HttpGet]
        [Route("GetResignationClearanceByEmployeeId/{EmployeeId}")]
        [Authorize]
        public async Task<IActionResult> GetResignationClearanceByEmployeeId(long EmployeeId)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _resignationClearanceService.GetResignationClearanceByEmployeeID(EmployeeId, accessToken, RemoteIpAddress));

        }


        [HttpGet]
        [Route("GetResignationByCompanyID/{companyId}")]
        [Authorize]
        public async Task<IActionResult> GetResignationClearanceByCompanyID(long companyId, [FromQuery] PaginationFilter filter)
        {

            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _resignationClearanceService.GetAllResignationClearanceByCompany(filter, companyId, accessToken, RemoteIpAddress));


        }


        //[HttpGet]
        //[Route("GetPendingResignationClearanceByEmployeeID/{EmployeeID}")]
        //public async Task<IActionResult> GetPendingResignationClearanceByEmployeeID(long EmployeeID)
        //{
        //    var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
        //    var identity = HttpContext.User.Identity as ClaimsIdentity;
        //    IEnumerable<Claim> claim = identity.Claims;
        //    var accessToken = Request.Headers["Authorization"];
        //    accessToken = accessToken.ToString().Replace("bearer", "").Trim();

        //    return Ok(await _resignationClearanceService.GetPendingResignationClearanceByEmployeeID(EmployeeID, accessToken, RemoteIpAddress));

        //}

        [HttpPost]
        [Route("ApprovePendingResignation")]
        public async Task<IActionResult> ApprovePendingResignationClearance([FromBody] ApproveResignationClearanceDTO request)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _resignationClearanceService.ApprovePendingResignationClearance(request, accessToken, RemoteIpAddress));

        }

        [HttpPost]
        [Route("DisapprovePendingResignation")]
        public async Task<IActionResult> DisapprovePendingResignationClearance([FromBody] DisapprovePendingResignationClearanceDTO request)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();

            return Ok(await _resignationClearanceService.DisapprovePendingResignationClearance(request, accessToken, RemoteIpAddress));

        }


    }
}
