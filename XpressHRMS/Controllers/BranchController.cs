﻿using Microsoft.AspNetCore.Authorization;
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

    public class BranchController : ControllerBase
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
                    response.ResponseMessage = "Branch Created Successfully";
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    return Ok(response);

                }
                else
                {
                    //response.Data = resp.Data;
                    response.ResponseMessage = "Internal Server Error";
                    response.ResponseCode = ResponseCode.InternalServer.ToString();
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
                    response.ResponseMessage = "branch Updated Successfully";
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    return Ok(response);

                }
                else
                {
                    //response.Data = resp.Data;
                    response.ResponseMessage = "Failed to Updated record";
                    response.ResponseCode = ResponseCode.InternalServer.ToString();
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
                    response.ResponseMessage = "Branch Deleted Successfully";
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    return Ok(response);

                }
                else
                {
                    
                    response.ResponseMessage = "Failed to delete record";
                    response.ResponseCode = ResponseCode.InternalServer.ToString();
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
                    response.ResponseMessage = "Branch Retrieved Successfully";
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    return Ok(response);

                }
                else
                {
                    //response.Data = resp.Data;
                    response.ResponseMessage = "No Record Found";
                    response.ResponseCode = ResponseCode.InternalServer.ToString();
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
                    response.ResponseMessage = "Branch Retrieved Successfully";
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    return Ok(response);

                }
                else
                {
                    //response.Data = resp.Data;
                    response.ResponseMessage = "No record found";
                    response.ResponseCode = ResponseCode.InternalServer.ToString();
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
