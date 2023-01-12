using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface IUnitService
    {
        Task<BaseResponse> ActivateUnit(DeleteUnitDTO payload);
        Task<BaseResponse> CreateUnit(CreateUnitDTO payload);
        Task<BaseResponse> DeleteUnit(DeleteUnitDTO payload);
        Task<BaseResponse> DisableUnit(DeleteUnitDTO payload);
        Task<BaseResponse> GetAllUnits(int CompanyID);
        Task<BaseResponse> GetUnitByID(int CompanyID, int UnitID);
        Task<BaseResponse> UpdateUnit(UpdateUnitDTO payload);
    }
}