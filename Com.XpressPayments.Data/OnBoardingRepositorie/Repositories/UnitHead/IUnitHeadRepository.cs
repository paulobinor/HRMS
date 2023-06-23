using Com.XpressPayments.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.UnitHead
{
    public interface IUnitHeadRepository
    {
        Task<dynamic> CreateUnitHead(CreateUnitHeadDTO create, string createdbyUserEmail);
        Task<dynamic> UpdateUnitHead(UpdateUnitHeadDTO update, string updatedbyUserEmail);
        Task<dynamic> DeleteUnitHead(DeleteUnitHeadDTO delete, string deletedbyUserEmail);
        Task<IEnumerable<UnitHeadDTO>> GetAllActiveUnitHead();
        Task<IEnumerable<UnitHeadDTO>> GetAllUnitHead();
        Task<UnitHeadDTO> GetUnitHeadById(long UnitHeadID);
        Task<UnitHeadDTO> GetUnitHeadByUserID(long UserID);
        Task<UnitHeadDTO> GetUnitHeadByCompany(long UserID, long companyId);
        Task<IEnumerable<UnitHeadDTO>> GetAllUnitHeadCompanyId(long UnitHeadID);
    }
}
