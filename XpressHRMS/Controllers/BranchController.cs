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

    public class BranchController : BaseController
    {
        private readonly IBranchService _branchService;
        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpPost("CreateBranch")]
        public async Task<IActionResult> CreateBranch([FromBody] CreateBranchDTO payload)
        {

            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                string CreatedBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                string CompanyID = claimsIdentity.FindFirst(ClaimTypes.SerialNumber)?.Value;
                payload.CreatedBy = CreatedBy;
                payload.CompanyID = CompanyID;

                return this.CustomResponse(await _branchService.CreateBranch(payload));
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPut("UpdateBranch")]
        public async Task<IActionResult> UpdateBranch([FromBody] UpdateBranchDTO UpdateBranch)
        {
            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                string updatedBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                UpdateBranch.UpdatedBy = updatedBy;
                return this.CustomResponse(await _branchService.UpdateBranch(UpdateBranch,RemoteIpAddress,RemotePort));
            }
            catch (Exception e)
            {
                return null;
            }
        }
        [HttpDelete("DeleteBranch")]
        public async Task<IActionResult> DeleteBranch(DeleteBranchDTO payload)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                string DeletedBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                //payload.CreatedBy = CreatedBy;
                return this.CustomResponse(await _branchService.DeleteBranch(payload));
            }
            catch (Exception e)
            {
                return null;
            }
        }
        [HttpGet("GetAllBranches")]
        public async Task<IActionResult> GetAllBranches(int CompanyID)
        {
            try
            {
                return this.CustomResponse(await _branchService.GetAllBranches(CompanyID));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet("GetBranchByID")]
        public async Task<IActionResult> GetBranchByID(int BranchID, string CompanyID)
        {
            try
            {
                return this.CustomResponse(await _branchService.GetBranchByID(BranchID, CompanyID));
            }
            catch (Exception e)
            {
                return null;
            }
        }
        
        [HttpPost("DisableBranch")]
        public async Task<IActionResult> DisableBranch(DisBranchDTO disable)
        {

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                return this.CustomResponse(await _branchService.DisableBranch(disable));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost("EnableBranch")]
        public async Task<IActionResult> ActivateBranch(EnBranchDTO enable)
        {

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                return this.CustomResponse(await _branchService.EnableBranch(enable));

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
