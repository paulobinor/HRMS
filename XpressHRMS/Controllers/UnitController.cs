using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Business.Services.ILogic;
using XpressHRMS.Data.DTO;
using XpressHRMS.Data.Enums;

namespace XpressHRMS.Controllers
{
    [Route("api/[controller]")]
    [Authorize]

    public class UnitController : BaseController
    {
        private readonly IUnitService _unitService;
        public UnitController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        [HttpPost("CreateUnit")]

        public async Task<IActionResult> CreateUnit([FromBody] CreateUnitDTO payload)
        {
            try
            {
                return this.CustomResponse(await _unitService.CreateUnit(payload));

            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPut("UpdateUnit")]

        public async Task<IActionResult> UpdateUnit([FromBody] UpdateUnitDTO payload)
        {

            try
            {
                return this.CustomResponse(await _unitService.UpdateUnit(payload));

            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpDelete("DeleteUnit")]
        public async Task<IActionResult> DeleteUnit([FromBody] DeleteUnitDTO payload)
        {
            try
            {
                return this.CustomResponse(await _unitService.DeleteUnit(payload));

            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpGet("GetAllUnits")]
        public async Task<IActionResult> GetAllUnits(int CompanyID)
        {
            try
            {
                return this.CustomResponse(await _unitService.GetAllUnits(CompanyID));

            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpGet("GetUnitByID")]
        public async Task<IActionResult> GetUnitByID(int CompanyID, int UnitID)
        {
            try
            {
                return this.CustomResponse(await _unitService.GetUnitByID(CompanyID, UnitID));

            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}
