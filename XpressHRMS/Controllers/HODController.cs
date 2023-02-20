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
    public class HodController : BaseController
    {
        private readonly IHodService _HODService;
        public HodController(IHodService HODService)
        {
            _HODService = HODService;
        }


        [HttpPost("CreateHOD")]
        public async Task<IActionResult> CreateHOD([FromBody] CreateHodDTO createHOD)
        {
            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {

                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                string CreatedBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                string companyid = claimsIdentity.FindFirst(ClaimTypes.SerialNumber)?.Value;
                createHOD.CreatedBy = CreatedBy;
                createHOD.CompanyID = companyid;

                return this.CustomResponse(await _HODService.CreateHOD(createHOD, RemoteIpAddress, RemotePort));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPut("UpdateHOD")]
        public async Task<IActionResult> UpdatePosition([FromBody] UpdateHodDTO UpdateHOD)
        {
            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {

                return this.CustomResponse(await _HODService.UpdateHOD(UpdateHOD, RemoteIpAddress, RemotePort));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpDelete("DeleteHOD")]
        public async Task<IActionResult> DeleteHOD(DelHodDTO DelHOD)
        {
            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {

                return this.CustomResponse(await _HODService.DeleteHOD(DelHOD, RemoteIpAddress, RemotePort));

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPost("ActivateHOD")]
        public async Task<IActionResult> ActivateHOD(EnableHodDTO enable)
        {
            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {

                return this.CustomResponse(await _HODService.ActivateHOD(enable, RemoteIpAddress, RemotePort));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost("DisableHOD")]
        public async Task<IActionResult> DisableHOD(DisableHodDTO disable)
        {
            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {

                return this.CustomResponse(await _HODService.DisableHOD(disable, RemoteIpAddress, RemotePort));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("GetAllHOD")]
        public async Task<IActionResult> GetAllHOD(string CompanyID)
        {
            try
            {
                return this.CustomResponse(await _HODService.GetAllHOD(CompanyID));

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpGet("GetAllHODByID")]
        public async Task<IActionResult> GetAllHODByID(string CompanyID, int HodID, int DepartmentID)
        {
            try
            {
                return this.CustomResponse(await _HODService.GetHODByID(CompanyID, HodID, DepartmentID));

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
