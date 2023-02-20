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
    //[Authorize]

    public class BankController : BaseController
    {
        private readonly IBankService _bankService;
        public BankController(IBankService bankService)
        {
            _bankService = bankService;
        }

        //[HttpPost("CreateBank")]
        //public async Task<IActionResult> CreateBank([FromBody] CreateBankDTO payload)
        //{

        //    try
        //    {
        //        return this.CustomResponse(await _bankService.CreateBank(payload));

        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}

        //[HttpPut("UpdateBank")]
        //public async Task<IActionResult> UpdateBank([FromBody] UpdateBankDTO payload)
        //{
        //    try
        //    {
        //        return this.CustomResponse(await _bankService.UpdateBank(payload));

        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}

        //[HttpGet("GetAllBanks")]
        //public async Task<IActionResult> GetAllBanks()
        //{
        //    try
        //    {
        //        return this.CustomResponse(await _bankService.GetAllBanks());

        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}
        //[HttpGet("GetBankByID")]
        //public async Task<IActionResult> GetBankByID(int bankid)
        //{
        //    try
        //    {
        //        return this.CustomResponse(await _bankService.GetBankByID(bankid));

        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}


        //stoppppppppp


        //private readonly IBankService _bankService;
        //public BankController(IBankService bankService)
        //{
        //    _bankService = bankService;
        //}

        //[HttpPost("CreateBank")]
        //public async Task<IActionResult> CreateBank([FromBody] CreateBankDTO payload)
        //{

        //    BaseResponse response = new BaseResponse();

        //    string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();

        //    try
        //    {
        //        var resp = await _bankService.CreateBank(payload);
        //        if (resp.Data != null)
        //        {
        //            response.Data = resp.Data;

        //            response.ResponseMessage = "Bank Created Successfully";
        //            response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
        //            return Ok(response);

        //        }
        //        else
        //        {
        //            response.ResponseMessage = "Internal Server Error";
        //            response.ResponseCode = ResponseCode.Already_Exist.ToString("D").PadLeft(2, '0');
        //            //response.Data = resp.Data;
        //            return Ok(response);
        //        }
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(response);
        //    }
        //}

        //[HttpPut("UpdateBank")]
        //public async Task<IActionResult> UpdateBank([FromBody] UpdateBankDTO payload)
        //{
        //    BaseResponse response = new BaseResponse();

        //    string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
        //    try
        //    {
        //        var resp = await _bankService.UpdateBank(payload);
        //        if (resp.Data != null)
        //        {
        //            response.Data = resp.Data;
        //            response.ResponseMessage = "Bank Updated Successfully";
        //            response.ResponseCode = ResponseCode.Ok.ToString();
        //            return Ok(response);

        //        }
        //        else
        //        {
        //            //response.Data = resp.Data;
        //            response.ResponseMessage = "Failed to Updated record";
        //            response.ResponseCode = ResponseCode.InternalServer.ToString();
        //            return Ok(response);
        //        }
        //        return Ok(response);
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(response);
        //    }
        //}

        //[HttpGet("GetAllBanks")]
        //public async Task<IActionResult> GetAllBanks()
        //{
        //    BaseResponse response = new BaseResponse();
        //    try
        //    {
        //        var resp = await _bankService.GetAllBanks();
        //        if (resp.Data != null)
        //        {
        //            response.Data = resp.Data;
        //            response.ResponseMessage = "Banks Retrieved Successfully";
        //            response.ResponseCode = ResponseCode.Ok.ToString();
        //            return Ok(response);

        //        }
        //        else
        //        {
        //            response.Data = resp.Data;
        //            response.ResponseMessage = "Banks Retrieved Successfully";
        //            response.ResponseCode = ResponseCode.Ok.ToString();
        //            return Ok(response);
        //        }
        //        return Ok(response);
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(response);
        //    }
        //}
        //[HttpGet("GetBankByID")]
        //public async Task<IActionResult> GetBankByID(int bankid )
        //{
        //    BaseResponse response = new BaseResponse();

        //    string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
        //    try
        //    {
        //        var resp = await _bankService.GetBankByID(bankid);
        //        if (resp.Data != null)
        //        {
        //            response.Data = resp.Data;
        //            response.ResponseMessage = "Bank Retrieved Successfully";
        //            response.ResponseCode = ResponseCode.Ok.ToString();
        //            return Ok(response);

        //        }
        //        else
        //        {
        //            //response.Data = resp.Data;
        //            response.ResponseMessage = "No record found";
        //            response.ResponseCode = ResponseCode.InternalServer.ToString();
        //            return Ok(response);
        //        }
        //        return Ok(response);
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(response);
        //    }
        //}
    }
}
