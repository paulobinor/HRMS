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
using XpressHRMS.Data.IRepository;

namespace XpressHRMS.Controllers
{
    [Route("api/[controller]")]
  // [Authorize]
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost("CreateDepartment")]

        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDTO payload)
        {
            try
            {
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
                return this.CustomResponse(await _departmentService.DeleteDepartment(payload));

            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpGet("GetAllDepartment")]
        public async Task<IActionResult> GetAllDepartment(int CompanyID)
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
        public async Task<IActionResult> GetAllDepartmentByID(int CompanyID, int DepartmentID)
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
        //public async Task<IActionResult> ActivateDepartment(DeleteDepartmentDTO payload)
        //{
        //    try
        //    {
        //        return this.CustomResponse(await _departmentService.ActivateDepartment(payload));

        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}

        //[HttpPost("DisableDepartment")]
        //public async Task<IActionResult> DisableDepartment(DeleteDepartmentDTO payload)
        //{
        //    try
        //    {
        //        return this.CustomResponse(await _departmentService.ActivateDepartment(payload));

        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}
    }
}
