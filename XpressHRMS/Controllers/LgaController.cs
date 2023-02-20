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
    public class LgaController : BaseController
    {
        private readonly ILgaService _LgaService;
        public LgaController(ILgaService LgaService)
        {
            _LgaService = LgaService;
        }

        [HttpGet("GetAllStateById")]
        public async Task<IActionResult> GetAllLgaByStateId(int StateID, int LGAID)
        {
            try
            {
                return this.CustomResponse(await _LgaService.GetAllLGAByState(StateID, LGAID));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("GetAllLga")]
        public async Task<IActionResult> GetAllLga(int StateID)
        {
            try
            {
                return this.CustomResponse(await _LgaService.GetAllLGA(StateID));

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
