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
    [Authorize]

    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
        [HttpPost("CreateCompany")]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDTO payload)
        {

            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();

            

            try
            {
                var resp = await _companyService.CreateCompany(payload, RemoteIpAddress, RemotePort);
                if (resp.Data != null)
                {
                    response.Data = resp.Data;
                    response.ResponseMessage = "Department Created Successfully";
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    return Ok(response);

                }
                else
                {
                    //response.Data = resp.Data;
                    response.ResponseMessage = "Internal Server Error";
                    response.ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0');
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(response);
            }
        }

        [HttpPut("UpdateCompany")]
        public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyDTO payload)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _companyService.UpdateCompany(payload, RemoteIpAddress, RemotePort);
                if (resp.Data != null)
                {
                    response.Data = resp.Data;
                    response.ResponseMessage = "Company Updated Successfully";
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    return Ok(response);

                }
                else
                {
                    //response.Data = resp.Data;
                    response.ResponseMessage = "Internal Server Error";
                    response.ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0');
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(response);
            }
        }
        [HttpDelete("DeleteCompany")]
        public async Task<IActionResult> DeleteCompany(int CompanyID)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _companyService.DeleteCompany(CompanyID, RemoteIpAddress, RemotePort);
                if (resp.Data != null)
                {
                    response.Data = resp.Data;
                    response.ResponseMessage = "Company Deleted Successfully";
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    return Ok(response);

                }
                else
                {
                    response.ResponseMessage = "Internal Server Error";
                    response.ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0');
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(response);
            }
        }
        [HttpGet("GetAllCompanies")]
        public async Task<IActionResult> GetAllCompanies()
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _companyService.GetAllCompanies();
                if (resp.Data != null)
                {
                    response.Data = resp.Data;
                    response.ResponseMessage = "Internal Server Error";
                    response.ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0');

                    return Ok(response);

                }
                else
                {
                    response.ResponseMessage = "Company Retrieved Successfully";
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(response);
            }
        }
        [HttpGet("GetAllCompaniesByID")]
        public async Task<IActionResult> GetAllCompaniesByID(int CompanyID)
        {
            BaseResponse response = new BaseResponse();
            
            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _companyService.GetCompanyByID(CompanyID);
                if (resp.Data != null)
                {
                    response.Data = resp.Data;
                    response.ResponseMessage = "Company Retrieved Successfully";
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    return Ok(response);

                }
                else
                {
                    
                    response.ResponseMessage = "No Record Found";
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(response);
            }
        }
        [HttpPost("ActivateCompany")]
        public async Task<IActionResult> ActivateCompany(int CompanyID)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _companyService.ActivateCompany(CompanyID, RemoteIpAddress, RemotePort);
                if (resp.Data != null)
                {
                    response.Data = resp.Data;
                    response.ResponseMessage = "Company Activated Successfully";
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    return Ok(response);

                }
                else
                {
                    response.ResponseMessage = "Internal Server Error";
                    response.ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0');
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(response);
            }
        }

        [HttpPost("DisableCompany")]
        public async Task<IActionResult> DisableCompany(int CompanyID)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _companyService.DisableCompany(CompanyID, RemoteIpAddress, RemotePort);
                if (resp.Data != null)
                {
                    response.Data = resp.Data;
                    response.ResponseMessage = "Company Disabled Successfully";
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    return Ok(response);

                }
                else
                {
                    response.ResponseMessage = "Internal Server Error";
                    response.ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0');
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(response);
            }
        }

    }
}
