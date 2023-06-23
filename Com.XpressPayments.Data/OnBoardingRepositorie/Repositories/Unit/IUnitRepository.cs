using Com.XpressPayments.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.Unit
{
    public interface IUnitRepository
    {
        Task<dynamic> CreateUnit(CreateUnitDTO unit, string createdbyUserEmail);
        Task<dynamic> UpdateUnit(UpdateUnitDTO unit, string updatedbyUserEmail);
        Task<dynamic> DeleteUnit(DeleteUnitDTO unit, string deletedbyUserEmail);
        Task<IEnumerable<UnitDTO>> GetAllActiveUnit();
        Task<IEnumerable<UnitDTO>> GetAllUnit();
        Task<UnitDTO> GetUnitById(long UnitID);
        Task<UnitDTO> GetUnitByName(string UnitName);
        Task<UnitDTO> GetUnitByCompany(string UnitName, int companyId);
        Task<IEnumerable<UnitDTO>> GetAllUnitCompanyId(long CompanyId);
    }
}
