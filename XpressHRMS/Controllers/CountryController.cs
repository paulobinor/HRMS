using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class CountryController : BaseController
    {
        private readonly ICountryService _CountryService;
        public CountryController(ICountryService CountryService)
        {
            _CountryService = CountryService;
        }

        [HttpGet("GetAllCountry")]
        public async Task<IActionResult> GetAllCountry()
        {
            try
            {
                return this.CustomResponse(await _CountryService.GetAllCounytries());

            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
