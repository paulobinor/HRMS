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

        [HttpPost("CreateBank")]
        public async Task<IActionResult> CreateBank([FromBody] CreateBankDTO payload)
        {

            try
            {
                return this.CustomResponse(await  _bankService.CreateBank(payload));

            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPut("UpdateBank")]
        public async Task<IActionResult> UpdateBank([FromBody] UpdateBankDTO payload)
        {
            try
            {
                return this.CustomResponse(await _bankService.UpdateBank(payload));

            }
            catch (Exception e)
            {
                return null;
            }
        }
       
        [HttpGet("GetAllBanks")]
        public async Task<IActionResult> GetAllBanks()
        {
            try
            {
                return this.CustomResponse(await _bankService.GetAllBanks());

            }
            catch (Exception e)
            {
                return null;
            }
        }
        [HttpGet("GetBankByID")]
        public async Task<IActionResult> GetBankByID(int bankid )
        {
            try
            {
                return this.CustomResponse(await _bankService.GetBankByID(bankid));

            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
