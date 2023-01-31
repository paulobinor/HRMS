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
                return this.CustomResponse(await _branchService.CreateBranch(payload));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPut("UpdateBranch")]
        public async Task<IActionResult> UpdateBranch([FromBody] UpdateBranchDTO payload)
        {
            try
            {
                return this.CustomResponse(await _branchService.UpdateBranch(payload));
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
            catch (Exception e)
            {
                return null;
            }
        }
        [HttpGet("GetBranchByID")]
        public async Task<IActionResult> GetBranchByID(DeleteBranchDTO payload)
        {
            try
            {
                return this.CustomResponse(await _branchService.GetBranchByID(payload));
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
