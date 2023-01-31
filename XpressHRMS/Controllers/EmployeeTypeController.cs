using Microsoft.AspNetCore.Authorization;
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
    //[Authorize]

    public class EmployeeTypeController : BaseController
    {
        private readonly IEmployeeTypeService _EmployeeTypeService;
        public EmployeeTypeController(IEmployeeTypeService EmployeeTypeService)
        {
            _EmployeeTypeService = EmployeeTypeService;
        }

        [HttpPost("CreateEmployeeType")]
        public async Task<IActionResult> CreateEmployeeType([FromBody] CreateEmployeeTypeDTO CraeteEmployeeType)
        {

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                //var resp = await _EmployeeTypeService.CreateEmployeeType(CraeteEmployeeType, RemoteIpAddress, RemotePort);
                return this.CustomResponse(await _EmployeeTypeService.CreateEmployeeType(CraeteEmployeeType, RemoteIpAddress, RemotePort));

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPut("UpdateEmployeeType")]
        public async Task<IActionResult> UpdateEmployeeType([FromBody] UpdateEmployeeTypeDTO UpdateEmployeeType)
        {
            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                //var resp = await _EmployeeTypeService.CreateEmployeeType(CraeteEmployeeType, RemoteIpAddress, RemotePort);
                return this.CustomResponse(await _EmployeeTypeService.UpdateEmployeeType(UpdateEmployeeType, RemoteIpAddress, RemotePort));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpDelete("DeleteEmployeeType")]
        public async Task<IActionResult> DeleteEmployeeType(DelEmployeeTypeDTO DelEmployeeType)
        {
            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                return this.CustomResponse(await _EmployeeTypeService.DeleteEmployeeType(DelEmployeeType, RemoteIpAddress, RemotePort));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("GetAllEmployeeType")]
        public async Task<IActionResult> GetAllEmployeeType(int CompanyID)
        {
           
            try
            {
                return this.CustomResponse(await _EmployeeTypeService.GetAllEmployeeType(CompanyID));

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpGet("GetAllEmployeeTypeByID")]
        public async Task<IActionResult> GetAllEmployeeTypeByID(int CompanyID, int EmployeeTypeID)
        {
            try
            {
                return this.CustomResponse(await _EmployeeTypeService.GetEmployeeTypeByID(CompanyID, EmployeeTypeID));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //[HttpPost("ActivateEmployeeType")]
        //public async Task<IActionResult> ActivateEmployeeType(int EmployeeTypeID, int CompanyID)
        //{
        //    BaseResponse response = new BaseResponse();

        //    string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
        //    try
        //    {
        //        var resp = await _EmployeeTypeService.ActivateEmployeeType(EmployeeTypeID, CompanyID, RemoteIpAddress, RemotePort);
        //        if (resp.Data != null)
        //        {
        //            response.Data = resp.Data;
        //            response.ResponseMessage = "EmployeeType Activated Successfully";
        //            response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
        //            return Ok(response);

        //        }
        //        else
        //        {
        //            //response.Data = resp.Data;
        //            response.ResponseMessage = "Internal Server Error";
        //            response.ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0');
        //            return Ok(response);
        //        }
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(response);
        //    }
        //}

        //[HttpPost("DisableEmployeeType")]
        //public async Task<IActionResult> DisableEmployeeType(int EmployeeTypeID, int CompanyID)
        //{
        //    BaseResponse response = new BaseResponse();

        //    string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
        //    try
        //    {
        //        var resp = await _EmployeeTypeService.DisableEmployeeType(EmployeeTypeID, CompanyID, RemoteIpAddress, RemotePort);
        //        if (resp.Data != null)
        //        {
        //            response.Data = resp.Data;
        //            response.ResponseMessage = "EmployeeType Disabled Successfully";
        //            response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
        //            return Ok(response);

        //        }
        //        else
        //        {
        //            //response.Data = resp.Data;
        //            response.ResponseMessage = "Internal Server Error";
        //            response.ResponseCode = ResponseCode.InternalServer.ToString();
        //            return Ok(response);
        //        }
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(response);
        //    }
        //}


    }
}
