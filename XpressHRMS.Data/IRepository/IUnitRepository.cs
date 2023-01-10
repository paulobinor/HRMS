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
        Task<IEnumerable<UnitDTO>> GetAllUnits(UnitDTO payload);
        Task<IEnumerable<UnitDTO>> GetUnitByID(int UnitID);
        Task<int> UpdateUnit(UpdateUnitDTO payload);
    }
}