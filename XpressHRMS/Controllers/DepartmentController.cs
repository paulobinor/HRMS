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
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost("CreateDepartment")]
        public async Task<IActionResult> CreateDepartment(DepartmentDTO payload)
        {
            BaseResponse response = new BaseResponse();
            try
            {             
                var resp = await _departmentService.CreateDepartment(payload);
                if (resp != null)
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = "";
                    return Ok(response);

                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(response);
            }
        }


        [HttpPut("UpdateDepartment")]
        public async Task<IActionResult> UpdateDepartment(UpdateDepartmentDTO payload)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _departmentService.UpdateDepartment(payload);
                if (resp != null)
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = "";
                    return Ok(response);

                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(response);
            }
        }


        [HttpDelete("DeleteDepartment")]
        public async Task<IActionResult> DeleteDepartment(DeleteDepartmentDTO payload)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _departmentService.DeleteDepartment(payload);
                if (resp != null)
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = "";
                    return Ok(response);

                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(response);
            }
        }

        [HttpPut("ActivateDepartment")]
        public async Task<IActionResult> ActivateDepartment(DeleteDepartmentDTO payload)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _departmentService.ActivateDepartment(payload);
                if (resp != null)
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = "";
                    return Ok(response);

                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(response);
            }
        }

        [HttpPut("DisableDepartment")]
        public async Task<IActionResult> DisableDepartment(DeleteDepartmentDTO payload)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _departmentService.DisableDepartment(payload);
                if (resp != null)
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = "";
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
