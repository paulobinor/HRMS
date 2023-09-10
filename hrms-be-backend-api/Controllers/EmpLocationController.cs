//using Com.XpressPayments.Bussiness.Services.ILogic;
//using Com.XpressPayments.Bussiness.Services.Logic;
//using Com.XpressPayments.Data.DTOs;
//using Com.XpressPayments.Data.Enums;
//using Com.XpressPayments.Data.GenericResponse;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System.Linq;
//using System.Threading.Tasks;
//using System;

//namespace hrms_be_backend_api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EmpLocationController : ControllerBase
//    {
//        private readonly ILogger<EmpLocationController> _logger;
//        private readonly IEmpLocationService _EmpLocationService;

//        public EmpLocationController(ILogger<EmpLocationController> logger, IEmpLocationService EmpLocationService)
//        {
//            _logger = logger;
//            _EmpLocationService = EmpLocationService;
//        }


//        [HttpPost("CreateEmplocation")]
//        [Authorize]
//        public async Task<IActionResult> CreateEmplocation([FromBody] CreateEmpLocationDTO EmplocationDto)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.LocalPort.ToString()
//                };

//                return Ok(await _EmpLocationService.CreateEmpLocation(EmplocationDto, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : CreateEmplocation ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : CreateEmplocation ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }
//        }

//        [HttpPost("UpdateEmplocation")]
//        [Authorize]
//        public async Task<IActionResult> UpdateEmplocation([FromBody] UpdateEmpLocationDTO updateDto)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.LocalPort.ToString()
//                };

//                return Ok(await _EmpLocationService.UpdateEmpLocation(updateDto, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : UpdateEmplocation ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : UpdateEmplocation ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }
//        }

//        [HttpPost("DeleteEmplocation")]
//        [Authorize]
//        public async Task<IActionResult> DeleteLocation([FromBody] DeleteEmpLocationDTO deleteDto)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.LocalPort.ToString()
//                };

//                return Ok(await _EmpLocationService.DeleteEmpLocation(deleteDto, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : DeleteLocation ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : DeleteLocation ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }
//        }

//        [Authorize]
//        [HttpGet("GetAllActiveEmplocation")]
//        public async Task<IActionResult> GetAllActiveDepartments()
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.LocalPort.ToString()
//                };

//                return Ok(await _EmpLocationService.GetAllActiveEmpLocation(requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : GetAllActiveEmpLocation==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllActiveEmpLocation ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }

//        }

//        [Authorize]
//        [HttpGet("GetAllEmplocation")]
//        public async Task<IActionResult> GetAllEmplocation()
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.LocalPort.ToString()
//                };

//                return Ok(await _EmpLocationService.GetAllEmpLocation(requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : GetAllEmplocation ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllEmplocation ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }
//        }

//        [Authorize]
//        [HttpGet("GetEmplocationbyId")]
//        public async Task<IActionResult> GetEmplocationbyId(int EmpLocationID)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.LocalPort.ToString()
//                };

//                return Ok(await _EmpLocationService.GetEmpLocationbyId(EmpLocationID, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : GetEmplocationbyId ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetEmplocationbyId ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }

//        }

//        [Authorize]
//        [HttpGet("GetEmplocationbyCompanyId")]
//        public async Task<IActionResult> GetEmplocationbyCompanyId(long CompanyID)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.LocalPort.ToString()
//                };

//                return Ok(await _EmpLocationService.GetEmpLocationbyCompanyId(CompanyID, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : GetEmplocationbyCompanyId ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetEmplocationbyCompanyId ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }

//        }

//    }
//}
