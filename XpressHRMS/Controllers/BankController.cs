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

    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;
        public BankController(IBankService bankService)
        {
            _bankService = bankService;
        }

        [HttpPost("CreateBank")]
        public async Task<IActionResult> CreateBank([FromBody] CreateBankDTO payload)
        {

            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();



            try
            {
                var resp = await _bankService.CreateBank(payload);
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

        [HttpPut("UpdateBank")]
        public async Task<IActionResult> UpdateBank([FromBody] UpdateBankDTO payload)
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _bankService.UpdateBank(payload);
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
       
        [HttpGet("GetAllBanks")]
        public async Task<IActionResult> GetAllBanks()
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _bankService.GetAllBanks();
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
        [HttpGet("GetBankByID")]
        public async Task<IActionResult> GetBankByID(int bankid )
        {
            BaseResponse response = new BaseResponse();

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                var resp = await _bankService.GetBankByID(bankid);
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
