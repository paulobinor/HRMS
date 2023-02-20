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
    public class StateController : BaseController
    {
        private readonly IStateService _StateService;
        public StateController(IStateService StateService)
        {
            _StateService = StateService;
        }

        [HttpGet("GetAllStateByCountryId")]
        public async Task<IActionResult> GetAllState(int CountryID, int StateID)
        {
            try
            {
                return this.CustomResponse(await _StateService.GetAllStateCountryID(CountryID, StateID));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("GetAllState")]
        public async Task<IActionResult> GetAllState(int CountryID)
        {
            try
            {
                return this.CustomResponse(await _StateService.GetAllState(CountryID));

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
