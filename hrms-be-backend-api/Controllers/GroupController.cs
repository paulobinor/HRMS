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
//using Microsoft.AspNetCore.Http;
//using OfficeOpenXml;
//using System.Collections.Generic;
//using System.Net.NetworkInformation;

//namespace hrms_be_backend_api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class GroupController : ControllerBase
//    {
//        private readonly ILogger<GroupController> _logger;
//        private readonly IGroupService _GroupService;

//        public GroupController(ILogger<GroupController> logger, IGroupService GroupService)
//        {
//            _logger = logger;
//            _GroupService = GroupService;
//        }

//        [HttpPost("CreateGroup")]
//        [Authorize]
//        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDTO groupDto)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.RemotePort.ToString()
//                };

//                return Ok(await _GroupService.CreateGroup(groupDto, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : CreateGroup ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : CreateGroup ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }
//        }

//        [HttpPost("CreateGroupBulkUpload")]
//        [Authorize]
//        public async Task<IActionResult> CreateGroupBulkUpload(IFormFile payload)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.RemotePort.ToString()
//                };

//                return Ok(await _GroupService.CreateGroupBulkUpload(payload, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : CreateGroupBulkUpload ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : CreateGroupBulkUpload ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }
//        }

//        [HttpPost("UpdateGroup")]
//        [Authorize]
//        public async Task<IActionResult> UpdateGroup([FromBody] UpdateGroupDTO updateDto)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.RemotePort.ToString()
//                };

//                return Ok(await _GroupService.UpdateGroup(updateDto, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : UpdateGroup ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : UpdateGroup ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }
//        }

//        [HttpPost("DeleteGroup")]
//        [Authorize]
//        public async Task<IActionResult> DeleteGroup([FromBody] DeleteGroupDTO deleteDto)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.RemotePort.ToString()
//                };

//                return Ok(await _GroupService.DeleteGroup(deleteDto, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : DeleteGroup ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : DeleteGroup ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }
//        }

//        [Authorize]
//        [HttpGet("GetAllActiveGroup")]
//        public async Task<IActionResult> GetAllActiveGroup()
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.RemotePort.ToString()
//                };

//                return Ok(await _GroupService.GetAllActiveGroup(requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : GetAllActiveGroup ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllActiveGroup ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }

//        }

//        [Authorize]
//        [HttpGet("GetAllGROUP")]
//        public async Task<IActionResult> GetAllGroup()
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.RemotePort.ToString()
//                };

//                return Ok(await _GroupService.GetAllGroup(requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : GetAllGroup ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllGroup ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }
//        }

//        [Authorize]
//        [HttpGet("GetGroupById")]
//        public async Task<IActionResult> GetGroupById(int GroupID)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.RemotePort.ToString()
//                };

//                return Ok(await _GroupService.GetGroupbyId(GroupID, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : GetGroupbyId ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetGroupbyId ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }

//        }


//        [Authorize]
//        [HttpGet("GetGroupbyCompanyId")]
//        public async Task<IActionResult> GetGroupByCompanyId(long CompanyID)
//        {
//            var response = new BaseResponse();
//            try
//            {
//                var requester = new RequesterInfo
//                {
//                    Username = this.User.Claims.ToList()[2].Value,
//                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
//                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
//                    IpAddress =  Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
//                    Port = Request.HttpContext.Connection.RemotePort.ToString()
//                };

//                return Ok(await _GroupService.GetGroupbyCompanyId(CompanyID, requester));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured: ControllerMethod : GetGroupbyCompanyId ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetGroupbyCompanyId ==> {ex.Message}";
//                response.Data = null;
//                return Ok(response);
//            }

//        }


//    }
//}
