using Com.XpressPayments.Api.Wrappers;
using Com.XpressPayments.Bussiness.Services.ILogic;
using Com.XpressPayments.Bussiness.ViewModels;
using Com.XpressPayments.Data.DTOs.Account;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.GenericResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace Com.XpressPayments.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _logger = logger;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var response = new LoginResponse();
            try
            {
                var IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString();
                var Port = Request.HttpContext.Connection.LocalPort.ToString();
                return Ok(await _authService.Login(login, IpAddress, Port));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : Login ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : Login ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel refresh)
        {
            //use fv to validate RefreshTokenModel input
            var response = new RefreshTokenResponse();
            try
            {
                var IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString();
                var Port = Request.HttpContext.Connection.LocalPort.ToString();
                return Ok(await _authService.RefreshToken(refresh, IpAddress, Port));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : RefreshToken ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : RefreshToken ==> {ex.Message}";
                return Ok(response);
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(LogoutDto logout)
        {
            var response = new BaseResponse();
            try
            {
                var IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString();
                var Port = Request.HttpContext.Connection.LocalPort.ToString();
                return Ok(await _authService.Logout(logout, IpAddress, Port));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : Logout ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : Logout ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }


        [HttpPost("CreateUser")]
        [Authorize]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _authService.CreateUser(userDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : CreateUser ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : CreateUser ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }


        [HttpPost("CreateUserBulkUpload")]
        [Authorize]
        public async Task<IActionResult> UploadTempleTransactions(IFormFile batchTransactions)
        {
            try
            {
                if (batchTransactions?.Length > 0)
                {
                    List<CreateUserDto> batchList = new();
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var stream = batchTransactions.OpenReadStream();
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets.First();
                        var rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount - 1; row++)
                        {
                            var FirstName = worksheet.Cells[row, 1].Value?.ToString();
                            var MiddleName = worksheet.Cells[row, 2].Value?.ToString();
                            var LastName = worksheet.Cells[row, 3].Value?.ToString();
                            var OfficialMail = worksheet.Cells[row, 5].Value?.ToString();
                            var Email = worksheet.Cells[row, 4].Value?.ToString(); 
                            var PhoneNumber = worksheet.Cells[row, 6].Value?.ToString();
                            var RoleId = worksheet.Cells[row, 7].Value?.ToString();
                            var CompanyId = worksheet.Cells[row, 20].Value?.ToString();
                            var DepartmentId = worksheet.Cells[row, 8].Value?.ToString();
                            var UnitID = worksheet.Cells[row, 9].Value?.ToString();
                            var UnitHeadID = worksheet.Cells[row, 10].Value?.ToString();
                            var HodID = worksheet.Cells[row, 11].Value?.ToString();
                            var GradeID = worksheet.Cells[row, 12].Value?.ToString();
                            var EmployeeTypeID = worksheet.Cells[row, 13].Value?.ToString();
                            var PositionID = worksheet.Cells[row, 14].Value?.ToString();
                            var DOB = worksheet.Cells[row, 15].Value?.ToString();
                            var BranchID = worksheet.Cells[row, 16].Value?.ToString();
                            var EmploymentStatusID = worksheet.Cells[row, 17].Value?.ToString();
                            var GroupID = worksheet.Cells[row, 18].Value?.ToString();
                            var ResumptionDate = worksheet.Cells[row, 19].Value?.ToString();
                            



                            CreateUserDto user = new()
                            {
                                FirstName = FirstName,
                                MiddleName = MiddleName,
                                LastName = LastName,
                                OfficialMail = OfficialMail,
                                Email = Email,
                                PhoneNumber = PhoneNumber,
                                RoleId = Convert.ToInt32(RoleId),
                                DepartmentId = Convert.ToInt32(DepartmentId),
                                UnitID = Convert.ToInt32(UnitID),
                                UnitHeadID = Convert.ToInt32(UnitHeadID),
                                HodID = Convert.ToInt32(HodID),
                                GradeID = Convert.ToInt32(GradeID),
                                EmployeeTypeID = Convert.ToInt32(EmployeeTypeID),
                                PositionID = Convert.ToInt32(PositionID),
                                DOB = DOB,
                                BranchID = Convert.ToInt32(BranchID),
                                EmploymentStatusID = Convert.ToInt32(EmploymentStatusID),
                                GroupID = Convert.ToInt32(GroupID),
                                ResumptionDate = ResumptionDate,
                                CompanyId = Convert.ToInt32(CompanyId)
                            };

                            var requester = new RequesterInfo
                            {
                                Username = this.User.Claims.ToList()[2].Value,
                                UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                                RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                                IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                                Port = Request.HttpContext.Connection.LocalPort.ToString()
                            };
                            var res = await _authService.CreateUser(user, requester);

                        }

                    }
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            return null;
        }

        [HttpPost("UpdateUser")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateDto)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _authService.UpdateUser(updateDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : UpdateUserDto ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : UpdateUserDto ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }


        [HttpPost("SendEmailForPasswordChange")]
        [AllowAnonymous]
        [Authorize]
        public async Task<IActionResult> SendEmailForPasswordChange(RequestPasswordChange request)
        {
            var response = new BaseResponse();
            try
            {
                var IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString();
                var Port = Request.HttpContext.Connection.LocalPort.ToString();
                return Ok(await _authService.SendEmailForPasswordChange(request, IpAddress, Port));

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : SendEmailForPasswordChange ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : SendEmailForPasswordChange ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }


        }

        [HttpPost("ChangePassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePassword)
        {
            var response = new BaseResponse();

            try
            {
                var IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString();
                var Port = Request.HttpContext.Connection.LocalPort.ToString();
                return Ok(await _authService.ChangePassword(changePassword, IpAddress, Port));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : ChangePassword ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : ChangePassword ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _authService.GetAllUsers(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllUsers ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllUsers ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [Authorize]
        [HttpGet("GetAllUsersPendingApproval")]
        public async Task<IActionResult> GetAllUsersPendingApproval()
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _authService.GetAllUsersPendingApproval(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetUsersPendingApproval ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetUsersPendingApproval ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("ApproveUser")]
        [Authorize]
        public async Task<IActionResult> ApproveUser(ApproveUserDto approveUser)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _authService.ApproveUser(approveUser, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : ApproveUser ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : ApproveUser ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [HttpPost("DisapproveUser")]
        [Authorize]
        public async Task<IActionResult> DisapproveUser(DisapproveUserDto disapproveUser)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _authService.DisapproveUser(disapproveUser, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DisapproveUser ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DisapproveUser ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [HttpPost("DeactivateUser")]
        [Authorize]
        public async Task<IActionResult> DeactivateUser(DeactivateUserDto deactivateUser)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _authService.DeactivateUser(deactivateUser, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DeactivateUser ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DeactivateUser ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("Re-activateUser")]
        [Authorize]
        public async Task<IActionResult> ReactivateUser(ReactivateUserDto reactivateUser)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _authService.ReactivateUser(reactivateUser, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : ReactivateUser ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : ReactivateUser ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("UnblockAccount")]
        [Authorize]
        public async Task<IActionResult> UnblockAccount(UnblockAccountDto unblockUser)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _authService.UnblockAccount(unblockUser, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : unblockUser ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : unblockUser ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetAllUsersbyDeptId")]
        public async Task<IActionResult> GetAllUsersbyDeptId([FromQuery] long DepartmentId)
        {
            var response = new BaseResponse();
            try
            {
                var requester = new RequesterInfo
                {
                    Username = this.User.Claims.ToList()[2].Value,
                    UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                    RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                    IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                    Port = Request.HttpContext.Connection.LocalPort.ToString()
                };

                return Ok(await _authService.GetAllUsersbyDeptId(DepartmentId, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllUsersbyDeptId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllUsersbyDeptId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }
    }
}
