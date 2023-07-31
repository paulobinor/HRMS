using System.Collections.Generic;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Data.IRepository
{
    public interface IUnitRepository
    {
        Task<int> ActivateUnit(int UnitID, int CompanyID);
        Task<int> CreateUnit(CreateUnitDTO payload);
        Task<int> DeleteUnit(int UnitID, int CompanyID);
        Task<int> DisableUnit(int UnitID, int CompanyID);
        Task<List<UnitDTO>> GetAllUnits(int CompanyID);
        Task<UnitDTO> GetUnitByID(int UnitID, int CompanyID);
        Task<int> UpdateUnit(UpdateUnitDTO payload);
    }
}