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
    [ApiController]
    [Authorize]

    public class GradeController : ControllerBase
    {
        private readonly IGradeService _GradeService;
        public GradeController(IGradeService GradeService)
        {
            _GradeService = GradeService;
        }

        [HttpPost("CreateGrade")]
        public async Task<IActionResult> CreateGrade([FromBody] CreateGradeDTO CraeteGrade)
        {

            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();



            try
            {
                var resp = await _GradeService.CreateGrade(CraeteGrade, RemoteIpAddress, RemotePort);
                if (resp.Data != null)
                {
                    response.Data = resp;
                    //response.ResponseCode = ResponseCode.Ok.ToString();
                    //response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(response);
            }
        }

        [HttpPut("UpdateGrade")]
        public async Task<IActionResult> UpdateGrade([FromBody] UpdateGradeDTO UpdateGrade)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _GradeService.UpdateGrade(UpdateGrade, RemoteIpAddress, RemotePort);
                if (resp.Data != null)
                {
                    response.Data = resp;
                    //response.ResponseCode = ResponseCode.Ok.ToString();
                    //response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);
                }
                //return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(response);
            }
        }

        [HttpDelete("DeleteGrade")]
        public async Task<IActionResult> DeleteGrade(DelGradeDTO DelGrade)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _GradeService.DeleteGrade(DelGrade, RemoteIpAddress, RemotePort);
                if (resp.Data != null)
                {
                    response.Data = resp;
                    //response.ResponseCode = ResponseCode.Ok.ToString();
                    //response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);
                }
                //return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(response);
            }
        }

        [HttpGet("GetAllGrade")]
        public async Task<IActionResult> GetAllPosition(int CompanyID)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _GradeService.GetAllGrade(CompanyID);
                if (resp.Data != null)
                {
                    response.Data = resp;
                    //response.ResponseCode = ResponseCode.Ok.ToString();
                    //response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);
                }
                //return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(response);
            }
        }


        [HttpGet("GetAllGradeByID")]
        public async Task<IActionResult> GetAllGradeByID(int CompanyID, int GradeID)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _GradeService.GetGradeByID(CompanyID, GradeID);
                if (resp.Data != null)
                {
                    response.Data = resp;
                    //response.ResponseCode = ResponseCode.Ok.ToString();
                    //response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(response);
            }
        }

        [HttpPost("ActivateGrade")]
        public async Task<IActionResult> ActivateGrade(int GradeID, int CompanyID)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _GradeService.ActivateGrade(GradeID, CompanyID, RemoteIpAddress, RemotePort);
                if (resp.Data != null)
                {
                    response.Data = resp;
                    //response.ResponseCode = ResponseCode.Ok.ToString();
                    //response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(response);
            }
        }

        [HttpPost("DisableGrade")]
        public async Task<IActionResult> DisableGrade(int GradeID, int CompanyID)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _GradeService.DisableGrade(GradeID, CompanyID, RemoteIpAddress, RemotePort);
                if (resp.Data != null)
                {
                    response.Data = resp;
                    //response.ResponseCode = ResponseCode.Ok.ToString();
                    //response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(response);
            }
        }

    }
}
