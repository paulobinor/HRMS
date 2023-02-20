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
using XpressHRMS.Data.IRepository;

namespace XpressHRMS.Controllers
{
    [Route("api/[controller]")]
  // [Authorize]
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentService _departmentService;
        private readonly IAuditTrailRepository _auditTrailRepository;
        public DepartmentController(IDepartmentService departmentService, IAuditTrailRepository auditTrailRepository)
        {
            _departmentService = departmentService;
            _auditTrailRepository = auditTrailRepository;
        }

        [HttpPost("CreateDepartment")]

        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDTO payload)
        {
            try
            {

                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                string CreatedBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                string CompanyID = claimsIdentity.FindFirst(ClaimTypes.SerialNumber)?.Value;
                payload.CreatedBy = CreatedBy;
                payload.CompanyID = CompanyID;


                return this.CustomResponse(await _departmentService.CreateDepartment(payload));

            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPut("UpdateDepartment")]

        public async Task<IActionResult> UpdateDepartment([FromBody] UpdateDepartmentDTO payload)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                string updatedBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                payload.UpdatedBy= updatedBy;
                return this.CustomResponse(await _departmentService.UpdateDepartment(payload));

            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpDelete("DeleteDepartment")]
        public async Task<IActionResult> DeleteDepartment([FromBody] DeleteDepartmentDTO payload)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                string DeletedBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                payload.DeletedBy = DeletedBy;
                return this.CustomResponse(await _departmentService.DeleteDepartment(payload));

            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpGet("GetAllDepartment")]
        public async Task<IActionResult> GetAllDepartment(string CompanyID)
        {
            try
            {
                return this.CustomResponse(await _departmentService.GetAllDepartments(CompanyID));

            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpGet("GetAllDepartmentByID")]
        public async Task<IActionResult> GetAllDepartmentByID(string CompanyID, int DepartmentID)
        {
            try
            {
                return this.CustomResponse(await _departmentService.GetAllDepartmentByID(CompanyID, DepartmentID));

            }
            catch (Exception e)
            {
                return null;
            }
        }



        //[HttpPost("ActivateDepartment")]
        //public async Task<IActionResult> ActivateDepartment(int DepartmentID, string CompanyID)
        //{
        //    try
        //    {
        //        var claimsIdentity = this.User.Identity as ClaimsIdentity;
        //        string EnableBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
        //        //payload.CreatedBy = CreatedBy;
        //        return this.CustomResponse(await _departmentService.ActivateDepartment(DepartmentID, CompanyID));

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //[HttpPost("DisableDepartment")]
        //public async Task<IActionResult> DeactivateDepartment(int DepartmentID, string CompanyID)
        //{
        //    try
        //    {
        //        var claimsIdentity = this.User.Identity as ClaimsIdentity;
        //        string DisableBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
        //        return this.CustomResponse(await _departmentService.DisableDepartment(DepartmentID, CompanyID, RemoteIpAddress, RemotePort));

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }
}
