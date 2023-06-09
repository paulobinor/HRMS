using Com.XpressPayments.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.HMO
{
    public interface IHMORepository
    {
        Task<dynamic> CreateHMO(CreateHMODTO create, string createdbyUserEmail);
        Task<dynamic> UpdateHMO(UpdateHMODTO HMO, string updatedbyUserEmail);
        Task<dynamic> DeleteHMO(DeleteHMODTO DEL, string deletedbyUserEmail);
        Task<IEnumerable<HMODTO>> GetAllActiveHMO();
        Task<IEnumerable<HMODTO>> GetAllHMO();
        Task<HMODTO> GetHMOById(long ID);
        Task<HMODTO> GetHMOByName(string HMONumber);
        Task<IEnumerable<HMODTO>> GetAllHMOCompanyId(long companyId);
    }
}
