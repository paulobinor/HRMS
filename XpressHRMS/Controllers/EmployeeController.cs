using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XpressHRMS.Business.Services.ILogic;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Controllers
{
    [Route("api/[controller]")]

    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService _EmployeeService;
        public EmployeeController(IEmployeeService EmployeeService)
        {
            _EmployeeService = EmployeeService;
        }

        //[HttpPost("CreateEmployee")]
        //public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDTO createEmp, int CompanyID)
        //{


        //    string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
        //    try
        //    {
        //        return this.CustomResponse(await _EmployeeService.CreateEmployee(createEmp, CompanyID));

        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}


        //stopppppppppp

        //[HttpPost("UploadBulkEmployee")]
        //public async Task<IActionResult> UploadBulkEmployee([FromForm] EmployeeUpload payload, int CompanyID)
        //{


        //    string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
        //    try
        //    {
        //        return this.CustomResponse(await _EmployeeService.CreateEmployeeBulk(payload, CompanyID));

        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}
    }
}
