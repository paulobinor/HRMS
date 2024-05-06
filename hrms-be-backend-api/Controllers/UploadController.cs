using hrms_be_backend_business.ILogic;
using hrms_be_backend_business.Logic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System.Security.Claims;

namespace hrms_be_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : BaseController
    {
        private readonly ILogger<UploadController> _logger;
        private readonly IUploadFileService _uploadFileService;
        private readonly IAuthService _authService;

        public UploadController(IUploadFileService uploadFileService, ILogger<UploadController> logger, IAuthService authService)
        {
            _logger = logger;
            _uploadFileService = uploadFileService;
            _authService = authService;
        }


        [HttpPost]
        [Route("file")]
        [Authorize]
        public async Task<IActionResult> UploadFile(IFormFile formFile)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            _logger.LogInformation($"Received GetPendingLeaveApprovals request. Payload: {JsonConvert.SerializeObject(new { formFile.FileName })} from remote address: {RemoteIpAddress}");

            var accessToken = Request.Headers["Authorization"].ToString().Split(" ").Last();
            var accessUser = await _authService.CheckUserAccess(accessToken, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return Unauthorized(new { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString() });

            }

            return Ok(await _uploadFileService.UploadFile(formFile, accessUser.data.OfficialMail));
        }
    }
}
