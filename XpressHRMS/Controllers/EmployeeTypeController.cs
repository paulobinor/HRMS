using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Business.Services.ILogic;
using XpressHRMS.Data.DTO;
using XpressHRMS.Data.Enums;

namespace XpressHRMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeTypeController : ControllerBase
    {
        private readonly IEmployeeTypeService _EmployeeTypeService;
        public EmployeeTypeController(IEmployeeTypeService EmployeeTypeService)
        {
            _EmployeeTypeService = EmployeeTypeService;
        }

        [HttpPost("CreateEmployeeType")]
        public async Task<IActionResult> CreateEmployeeType([FromBody] CreateEmployeeTypeDTO CraeteEmployeeType)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _EmployeeTypeService.CreateEmployeeType(CraeteEmployeeType, RemoteIpAddress, RemotePort);
                if (resp.Data != null)
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(response);
            }
        }


        [HttpPut("UpdateEmployeeType")]
        public async Task<IActionResult> UpdateEmployeeType([FromBody] UpdateEmployeeTypeDTO UpdateEmployeeType)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _EmployeeTypeService.UpdateEmployeeType(UpdateEmployeeType, RemoteIpAddress, RemotePort);
                if (resp.Data != null)
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);
                }
                //return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(response);
            }
        }

        [HttpDelete("DeleteEmployeeType")]
        public async Task<IActionResult> DeleteEmployeeType(DelEmployeeTypeDTO DelEmployeeType)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _EmployeeTypeService.DeleteEmployeeType(DelEmployeeType, RemoteIpAddress, RemotePort);
                if (resp.Data != null)
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);
                }
                //return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(response);
            }
        }

        [HttpGet("GetAllEmployeeType")]
        public async Task<IActionResult> GetAllEmployeeType()
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _EmployeeTypeService.GetAllEmployeeType();
                if (resp.Data != null)
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(response);
            }
        }


        [HttpGet("GetAllEmployeeTypeByID")]
        public async Task<IActionResult> GetAllEmployeeTypeByID(int CompanyID, int EmployeeTypeID)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _EmployeeTypeService.GetEmployeeTypeByID(CompanyID,EmployeeTypeID);
                if (resp.Data != null)
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(response);
            }
        }

        [HttpPost("ActivateEmployeeType")]
        public async Task<IActionResult> ActivateEmployeeType(int EmployeeTypeID)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _EmployeeTypeService.ActivateEmployeeType(EmployeeTypeID, RemoteIpAddress, RemotePort);
                if (resp.Data != null)
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(response);
            }
        }

        [HttpPost("DisableEmployeeType")]
        public async Task<IActionResult> DisableEmployeeType(int EmployeeTypeID)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _EmployeeTypeService.DisableEmployeeType(EmployeeTypeID, RemoteIpAddress, RemotePort);
                if (resp.Data != null)
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(response);
            }
        }


    }
}
