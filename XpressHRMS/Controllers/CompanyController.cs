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
    [Authorize]

    public class CompanyController : BaseController
    {
        private readonly ICompanyService _companyService;
        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
        [HttpPost("CreateCompany")]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDTO payload)
        {

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {

                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                string CreatedBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                payload.CreatedBy = CreatedBy;

                return this.CustomResponse(await _companyService.CreateCompany(payload, RemoteIpAddress, RemotePort));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPut("UpdateCompany")]
        public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyDTO payload)
        {
            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                string updatedBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                payload.UpdatedBy = updatedBy;
                return this.CustomResponse(await _companyService.UpdateCompany(payload, RemoteIpAddress, RemotePort));

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpDelete("DeleteCompany")]
        public async Task<IActionResult> DeleteCompany(int CompanyID)
        {
            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                string DeletedBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                //payload.CreatedBy = CreatedBy;
                return this.CustomResponse(await _companyService.DeleteCompany(CompanyID, DeletedBy, RemoteIpAddress, RemotePort));

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet("GetAllCompanies")]
        public async Task<IActionResult> GetAllCompanies()
        {
            try
            {
                return this.CustomResponse(await _companyService.GetAllCompanies());

            }
            catch (Exception e)
            {
                return null;
            }
        }
        [HttpGet("GetAllCompaniesByID")]
        public async Task<IActionResult> GetAllCompaniesByID(int CompanyID)
        {
            try
            {
                return this.CustomResponse(await _companyService.GetCompanyByID(CompanyID));

            }
            catch (Exception e)
            {
                return null;
            }
        }
        [HttpPost("ActivateCompany")]
        public async Task<IActionResult> ActivateCompany(int CompanyID)
        {

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                string EnableBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                //payload.CreatedBy = CreatedBy;
                return this.CustomResponse(await _companyService.ActivateCompany(CompanyID, EnableBy, RemoteIpAddress, RemotePort));

            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPost("DisableCompany")]
        public async Task<IActionResult> DisableCompany(int CompanyID)
        {

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {

                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                string DisableBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                //payload.CreatedBy = CreatedBy;
                return this.CustomResponse(await _companyService.DisableCompany(CompanyID, DisableBy, RemoteIpAddress, RemotePort));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
