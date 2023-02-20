using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class PositionController : BaseController
    {
        private readonly IPositionService _PositionService;
        public PositionController(IPositionService PositionService)
        {
            _PositionService = PositionService;
        }

        //[HttpPost("CreatePosition")]
        //public async Task<IActionResult> CreatePosition([FromBody] CreatePositionDTO CraetePosition)
        //{
        //    string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
        //    try
        //    {

        //        var claimsIdentity = this.User.Identity as ClaimsIdentity;
        //        string CreatedBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
        //        string companyid = claimsIdentity.FindFirst(ClaimTypes.SerialNumber)?.Value;
        //        CraetePosition.CreatedBy = CreatedBy;
        //        CraetePosition.CompanyID = companyid;

        //        return this.CustomResponse(await _PositionService.CreatePosition(CraetePosition, RemoteIpAddress, RemotePort));

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}


        //[HttpPut("UpdatePosition")]
        //public async Task<IActionResult> UpdatePosition([FromBody] UPdatePositionDTO UpdatePosition)
        //{
        //    string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
        //    try
        //    {
        //        //var claimsIdentity = this.User.Identity as ClaimsIdentity;
        //        //string UpadteBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
        //        //string companyid = claimsIdentity.FindFirst(ClaimTypes.SerialNumber)?.Value;
        //        //UpdatePosition.UpdatedByUpd = UpadteBy;
        //        //CraetePosition.CompanyID = companyid;
        //        return this.CustomResponse(await _PositionService.UpdatePosition(UpdatePosition, RemoteIpAddress, RemotePort));

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //[HttpDelete("DeletePosition")]
        //public async Task<IActionResult> DeletePosition(DeletePositionDTO DelPosition)
        //{
        //    string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
        //    try
        //    {
        //        var claimsIdentity = this.User.Identity as ClaimsIdentity;
        //        string DeletedBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
        //        //string companyid = claimsIdentity.FindFirst(ClaimTypes.SerialNumber)?.Value;
        //        DelPosition.DeletedBy = DeletedBy;
        //        return this.CustomResponse(await _PositionService.DeletePosition(DelPosition, RemoteIpAddress, RemotePort));

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //[HttpGet("GetAllPosition")]
        //public async Task<IActionResult> GetAllPosition(int CompanyID)
        //{
        //    try
        //    {
        //        return this.CustomResponse(await _PositionService.GetAllPositions(CompanyID));

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}


        //[HttpGet("GetAllPositionByID")]
        //public async Task<IActionResult> GetAllPositionByID(int CompanyID, int PositionID)
        //{
        //    try
        //    {
        //        return this.CustomResponse(await _PositionService.GetPositionByID(CompanyID, PositionID));

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //[HttpPost("ActivatePosition")]
        //public async Task<IActionResult> ActivatePosition(int PositionID, int CompanyID)
        //{
        //    string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
        //    try
        //    {

        //        return this.CustomResponse(await _PositionService.ActivatePosition(PositionID, CompanyID, RemoteIpAddress, RemotePort));

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //[HttpPost("DisablePosition")]
        //public async Task<IActionResult> DisablePosition(int PositionID, int CompanyID)
        //{
        //    string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
        //    try
        //    {

        //        return this.CustomResponse(await _PositionService.DisablePosition(PositionID, CompanyID, RemoteIpAddress, RemotePort));

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

    }
}
