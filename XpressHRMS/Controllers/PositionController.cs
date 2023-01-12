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
    //[Authorize]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _PositionService;
        public PositionController(IPositionService PositionService)
        {
            _PositionService = PositionService;
        }

        [HttpPost("CreatePosition")]
        public async Task<IActionResult> CreatePosition([FromBody] CreatePositionDTO CraetePosition)
        {

            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();



            try
            {
                var resp = await _PositionService.CreatePosition(CraetePosition, RemoteIpAddress, RemotePort);
                if (resp.Data != null)
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
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


        [HttpPut("UpdatePosition")]
        public async Task<IActionResult> UpdatePosition([FromBody] UPdatePositionDTO UpdatePosition)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _PositionService.UpdatePosition(UpdatePosition, RemoteIpAddress, RemotePort);
                if (resp.Data != null)
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
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

        [HttpDelete("DeletePosition")]
        public async Task<IActionResult> DeletePosition(DeletePositionDTO DelPosition)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _PositionService.DeletePosition(DelPosition, RemoteIpAddress, RemotePort);
                if (resp.Data != null)
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
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

        [HttpGet("GetAllPosition")]
        public async Task<IActionResult> GetAllPosition()
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _PositionService.GetAllPositions();
                if (resp.Data != null)
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
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

     
        [HttpGet("GetAllPositionByID")]
        public async Task<IActionResult> GetAllPositionByID(int CompanyID, int PositionID)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _PositionService.GetPositionByID(CompanyID, PositionID);
                if (resp.Data != null)
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
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

        [HttpPost("ActivatePosition")]
        public async Task<IActionResult> ActivatePosition(int PositionID)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _PositionService.ActivatePosition(PositionID, RemoteIpAddress, RemotePort);
                if (resp.Data != null)
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
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

        [HttpPost("DisablePosition")]
        public async Task<IActionResult> DisablePosition(int PositionID)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _PositionService.DisablePosition(PositionID, RemoteIpAddress, RemotePort);
                if (resp.Data != null)
                {
                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.ResponseMessage = resp.ResponseMessage;
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
