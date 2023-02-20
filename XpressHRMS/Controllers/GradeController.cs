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
    [Authorize]

    public class GradeController : BaseController
    {
        private readonly IGradeService _GradeService;
        public GradeController(IGradeService GradeService)
        {
            _GradeService = GradeService;
        }

        //[HttpPost("CreateGrade")]
        //public async Task<IActionResult> CreateGrade([FromBody] CreateGradeDTO CraeteGrade)
        //{


        //    string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();

        //    try
        //    {
        //        return this.CustomResponse(await _GradeService.CreateGrade(CraeteGrade,RemoteIpAddress,RemotePort));

        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}

        //[HttpPut("UpdateGrade")]
        //public async Task<IActionResult> UpdateGrade([FromBody] UpdateGradeDTO UpdateGrade)
        //{


        //    string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();

        //    try
        //    {
        //        return this.CustomResponse(await _GradeService.UpdateGrade(UpdateGrade, RemoteIpAddress, RemotePort));

        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}

        //[HttpDelete("DeleteGrade")]
        //public async Task<IActionResult> DeleteGrade(DelGradeDTO DelGrade)
        //{

        //    string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();

        //    try
        //    {
        //        return this.CustomResponse(await _GradeService.DeleteGrade(DelGrade, RemoteIpAddress, RemotePort));

        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}

        //[HttpGet("GetAllGrade")]
        //public async Task<IActionResult> GetAllGrade(int CompanyID)
        //{
        //    try
        //    {
        //        return this.CustomResponse(await _GradeService.GetAllGrade(CompanyID));

        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}


        //[HttpGet("GetAllGradeByID")]
        //public async Task<IActionResult> GetAllGradeByID(int CompanyID, int GradeID)
        //{
        //    try
        //    {
        //        return this.CustomResponse(await _GradeService.GetGradeByID(CompanyID, GradeID));

        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}

        //[HttpPost("ActivateGrade")]
        //public async Task<IActionResult> ActivateGrade(int GradeID, int CompanyID)
        //{
        //    BaseResponse response = new BaseResponse();

        //    string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
        //    try
        //    {
        //        var resp = await _GradeService.ActivateGrade(GradeID, CompanyID, RemoteIpAddress, RemotePort);
        //        if (resp.Data != null)
        //        {
        //            response.Data = resp.Data;
        //            response.ResponseMessage = "Position Activated Successfully";
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

        //[HttpPost("DisableGrade")]
        //public async Task<IActionResult> DisableGrade(int GradeID, int CompanyID)
        //{
        //    BaseResponse response = new BaseResponse();

        //    string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
        //    try
        //    {
        //        var resp = await _GradeService.DisableGrade(GradeID, CompanyID, RemoteIpAddress, RemotePort);
        //        if (resp.Data != null)
        //        {
        //            response.Data = resp.Data;
        //            response.ResponseMessage = "Position Disabled Successfully";
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

    }
}
