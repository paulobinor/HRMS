using Com.XpressPayments.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.HospitalProviders
{
    public interface IHospitalProvidersRepository
    {
        Task<dynamic> CreateHospitalProviders(CreateHospitalProvidersDTO create, string createdbyUserEmail);
        Task<dynamic> UpdateHospitalProviders(UpdateHospitalProvidersDTO update, string updatedbyUserEmail);
        Task<dynamic> DeleteHospitalProviders(DeleteHospitalProvidersDTO del, string deletedbyUserEmail);
        Task<IEnumerable<HospitalProvidersDTO>> GetAllActiveHospitalProviders();
        Task<IEnumerable<HospitalProvidersDTO>> GetAllHospitalProviders();
        Task<HospitalProvidersDTO> GetHospitalProvidersById(long ID);
        Task<HospitalProvidersDTO> GetHospitalProvidersByName(string ProvidersNames);
        Task<IEnumerable<HodDTO>> GetAllHospitalProvidersCompanyId(long companyId);
    }
}
