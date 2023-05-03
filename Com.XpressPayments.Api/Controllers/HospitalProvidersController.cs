using Com.XpressPayments.Bussiness.Services.ILogic;
using Com.XpressPayments.Bussiness.Services.Logic;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.GenericResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System;
using Com.XpressPayments.Data.DTOs.Account;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Collections.Generic;

namespace Com.XpressPayments.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalProvidersController : ControllerBase
    {
        private readonly ILogger<HospitalProvidersController> _logger;
        private readonly IHospitalProvidersService _HospitalProvidersService;

        public HospitalProvidersController(ILogger<HospitalProvidersController> logger, IHospitalProvidersService HospitalProvidersService)
        {
            _logger = logger;
            _HospitalProvidersService = HospitalProvidersService;
        }

        [HttpPost("CreateHospitalProviders")]
        [Authorize]
        public async Task<IActionResult> CreateHospitalProviders([FromBody] CreateHospitalProvidersDTO createDto)
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

                return Ok(await _HospitalProvidersService.CreateHospitalProviders(createDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : CreateHospitalProviders ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : CreateHospitalProviders ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("CreateHospitalProvidersBulkUpload")]
        [Authorize]
        public async Task<IActionResult> UploadTempleTransactions(IFormFile batchTransactions)
        {
            try
            {
                if (batchTransactions?.Length > 0)
                {
                    List<CreateHospitalProvidersDTO> batchList = new();
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var stream = batchTransactions.OpenReadStream();
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets.First();
                        var rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount - 1; row++)
                        {
                            var ProvidersNames = worksheet.Cells[row, 1].Value?.ToString();
                            var State = worksheet.Cells[row, 2].Value?.ToString();
                            var Town1 = worksheet.Cells[row, 3].Value?.ToString();
                            var Town2 = worksheet.Cells[row, 4].Value?.ToString();
                            var Address1 = worksheet.Cells[row, 5].Value?.ToString();
                            var Address2 = worksheet.Cells[row, 6].Value?.ToString();
                            var HospitalPlan = worksheet.Cells[row, 7].Value?.ToString();
                            var CompanyID = worksheet.Cells[row, 8].Value?.ToString();


                            CreateHospitalProvidersDTO Providers = new()
                            {
                                ProvidersNames = ProvidersNames,
                                State = State,
                                Town1 = Town1,
                                Town2 = Town2,
                                Address1 = Address1,
                                Address2 = Address2,
                                HospitalPlan = HospitalPlan,
                                CompanyID = Convert.ToInt32(CompanyID)
                            };

                            var requester = new RequesterInfo
                            {
                                Username = this.User.Claims.ToList()[2].Value,
                                UserId = Convert.ToInt64(this.User.Claims.ToList()[3].Value),
                                RoleId = Convert.ToInt64(this.User.Claims.ToList()[4].Value),
                                IpAddress = Request.HttpContext.Connection.LocalIpAddress?.ToString(),
                                Port = Request.HttpContext.Connection.LocalPort.ToString()
                            };
                            var res = await _HospitalProvidersService.CreateHospitalProviders(Providers, requester);

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        [HttpPost("UpdateHospitalProviders")]
        [Authorize]
        public async Task<IActionResult> UpdateHospitalProviders([FromBody] UpdateHospitalProvidersDTO updateDto)
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

                return Ok(await _HospitalProvidersService.UpdateHospitalProviders(updateDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : UpdateHospitalProviders ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : UpdateHospitalProviders ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [HttpPost("DeleteHospitalProviders")]
        [Authorize]
        public async Task<IActionResult> DeleteHospitalProviders([FromBody] DeleteHospitalProvidersDTO deleteDto)
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

                return Ok(await _HospitalProvidersService.DeleteHospitalProviders(deleteDto, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DeleteHospitalProviders ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DeleteHospitalProviders ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetAllActiveHospitalProviders")]
        public async Task<IActionResult> GetAllActiveHMO()
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

                return Ok(await _HospitalProvidersService.GetAllActiveHospitalProviders(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllActiveHospitalProviders ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllActiveHospitalProviders ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

        [Authorize]
        [HttpGet("GetAllHospitalProviders")]
        public async Task<IActionResult> GetAllHospitalProviders()
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

                return Ok(await _HospitalProvidersService.GetAllHospitalProviders(requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllHospitalProviders ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllHospitalProviders ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("GetHospitalProvidersbyId")]
        public async Task<IActionResult> GetHospitalProvidersbyId(int ID)
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

                return Ok(await _HospitalProvidersService.GetHospitalProvidersbyId(ID, requester));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetHospitalProvidersbyId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetHospitalProvidersbyId ==> {ex.Message}";
                response.Data = null;
                return Ok(response);
            }

        }

    }
}
