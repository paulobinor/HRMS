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

    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost("CreateDepartment")]

        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDTO payload)
        {
            BaseResponse response = new BaseResponse();
            try
            {             
                var resp = await _departmentService.CreateDepartment(payload);
                if (resp != null)
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

        [HttpPut("UpdateDepartment")]

        public async Task<IActionResult> UpdateDepartment([FromBody] UpdateDepartmentDTO payload)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _departmentService.UpdateDepartment(payload);
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
                    response.ResponseCode = ResponseCode.Exception.ToString();
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

        [HttpDelete("DeleteDepartment")]
        public async Task<IActionResult> DeleteDepartment([FromBody] DeleteDepartmentDTO payload)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _departmentService.DeleteDepartment(payload);
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
                    response.ResponseCode = ResponseCode.Exception.ToString();
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

        [HttpGet("GetAllDepartment")]
        public async Task<IActionResult> GetAllDepartment(int CompanyID)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _departmentService.GetAllDepartments(CompanyID);
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

        [HttpGet("GetAllDepartmentByID")]
        public async Task<IActionResult> GetAllDepartmentByID(int CompanyID, int DepartmentID)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _departmentService.GetAllDepartmentByID(CompanyID, DepartmentID);
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

        [HttpPost("ActivateDepartment")]
        public async Task<IActionResult> ActivateDepartment(int CompanyID)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _departmentService.ActivateDepartment(null);
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

        [HttpPost("DisableDepartment")]
        public async Task<IActionResult> DisableDepartment(int CompanyID)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _departmentService.DisableDepartment(null);
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
                    response.ResponseCode = ResponseCode.Exception.ToString();
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
