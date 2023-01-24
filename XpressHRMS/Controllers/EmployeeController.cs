using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XpressHRMS.Business.Services.ILogic;

namespace XpressHRMS.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _EmployeeService;
        public EmployeeController(IEmployeeService EmployeeService)
        {
            _EmployeeService = EmployeeService;
        }

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
        //            response.ResponseCode = ((int)ResponseCode.Ok).ToString();
        //            response.ResponseMessage = resp.ResponseMessage;
        //            return Ok(response);

        //        }
        //        else
        //        {
        //            response.Data = resp.Data;
        //            response.ResponseCode = ResponseCode.Ok.ToString();
        //            response.ResponseMessage = resp.ResponseMessage;
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
