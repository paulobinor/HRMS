using System.Collections.Generic;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Data.IRepository
{
    public interface IUnitRepository
    {
        Task<int> ActivateUnit(int UnitID);
        Task<int> CreateUnit(UnitDTO payload);
        Task<int> DeleteUnit(int UnitID);
        Task<int> DisableUnit(int UnitID);
        Task<List<UnitDTO>> GetAllUnits(UnitDTO payload);
        Task<List<UnitDTO>> GetUnitByID(int UnitID);
        Task<int> UpdateUnit(UpdateUnitDTO payload);
    }
}