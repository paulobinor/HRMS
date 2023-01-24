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

    public class UnitController : ControllerBase
    {
        private readonly IUnitService _unitService;
        public UnitController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        [HttpPost("CreateUnit")]

        public async Task<IActionResult> CreateUnit([FromBody] CreateUnitDTO payload)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _unitService.CreateUnit(payload);
                if (resp != null)
                {
                    response.Data = resp.Data;
                    return Ok(response);

                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(response);
            }
        }

        [HttpPut("UpdateUnit")]

        public async Task<IActionResult> UpdateUnit([FromBody] UpdateUnitDTO payload)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _unitService.UpdateUnit(payload);
                if (resp.Data != null)
                {
                    response.Data = resp.Data;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp.Data;
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(response);
            }
        }

        [HttpDelete("DeleteUnit")]
        public async Task<IActionResult> DeleteUnit([FromBody] DeleteUnitDTO payload)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _unitService.DeleteUnit(payload);
                if (resp.Data != null)
                {
                    response.Data = resp.Data;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp.Data;
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(response);
            }
        }

        [HttpGet("GetAllUnits")]
        public async Task<IActionResult> GetAllUnits(int CompanyID)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _unitService.GetAllUnits(CompanyID);
                if (resp.Data != null)
                {
                    response.Data = resp.Data;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp.Data;
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(response);
            }
        }

        [HttpGet("GetUnitByID")]
        public async Task<IActionResult> GetUnitByID(int CompanyID, int UnitID)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _unitService.GetUnitByID(CompanyID, UnitID);
                if (resp.Data != null)
                {
                    response.Data = resp.Data;
                    return Ok(response);

                }
                else
                {
                    response.Data = resp.Data;
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
