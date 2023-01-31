using System.Collections.Generic;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface IUnitService
    {
        Task<BaseResponse<DeleteUnitDTO>> ActivateUnit(DeleteUnitDTO payload);
        Task<BaseResponse<CreateUnitDTO>> CreateUnit(CreateUnitDTO payload);
        Task<BaseResponse<DeleteUnitDTO>> DeleteUnit(DeleteUnitDTO payload);
        Task<BaseResponse<DeleteUnitDTO>> DisableUnit(DeleteUnitDTO payload);
        Task<BaseResponse<List<UnitDTO>>> GetAllUnits(int CompanyID);
        Task<BaseResponse<UnitDTO>> GetUnitByID(int CompanyID, int UnitID);
        Task<BaseResponse<UpdateUnitDTO>> UpdateUnit(UpdateUnitDTO payload);
    }
}