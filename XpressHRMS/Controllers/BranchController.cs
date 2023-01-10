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

    public class BranchController : Controller
    {
        private readonly IBranchService _branchService;
        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpPost("CreateBranch")]
        public async Task<IActionResult> CreateBranch([FromBody] CreateBranchDTO payload)
        {

            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();



            try
            {
                var resp = await _branchService.CreateBranch(payload);
                if (resp.Data != null)
                {
                    response.Data = resp.Data;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp.Data;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(response);
            }
        }

        [HttpPut("UpdateBranch")]
        public async Task<IActionResult> UpdateBranch([FromBody] UpdateBranchDTO payload)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _branchService.UpdateBranch(payload);
                if (resp.Data != null)
                {
                    response.Data = resp.Data;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp.Data;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(response);
            }
        }
        [HttpDelete("DeleteBranch")]
        public async Task<IActionResult> DeleteBranch(DeleteBranchDTO payload)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _branchService.DeleteBranch(payload);
                if (resp.Data != null)
                {
                    response.Data = resp.Data;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp.Data;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(response);
            }
        }
        [HttpGet("GetAllBranches")]
        public async Task<IActionResult> GetAllBranches(int CompanyID)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _branchService.GetAllBranches(CompanyID);
                if (resp.Data != null)
                {
                    response.Data = resp.Data;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp.Data;
                    response.ResponseCode = ResponseCode.NotFound.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(response);
            }
        }
        [HttpGet("GetBranchByID")]
        public async Task<IActionResult> GetBranchByID(DeleteBranchDTO payload)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _branchService.GetBranchByID(payload);
                if (resp.Data != null)
                {
                    response.Data = resp.Data;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp.Data;
                    response.ResponseCode = ResponseCode.NotFound.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
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
