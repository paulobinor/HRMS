using Com.XpressPayments.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.HOD
{
    public  interface IHODRepository
    {
        Task<dynamic> CreateHOD(CreateHodDTO hod, string createdbyUserEmail);
        Task<dynamic> UpdateHOD(UpdateHodDTO hod, string updatedbyUserEmail);
        Task<dynamic> DeleteHOD(DeleteHodDTO hod, string deletedbyUserEmail);
        Task<IEnumerable<HodDTO>> GetAllActiveHODs();
        Task<IEnumerable<HodDTO>> GetAllHOD();
        Task<HodDTO> GetHODById(long HodID);
        Task<HodDTO> GetHODByName(long UserId);
        Task<HodDTO> GetHODByCompany(long UserId, long companyId);
        Task<IEnumerable<HodDTO>> GetAllHODCompanyId(long companyId);





    }
}
