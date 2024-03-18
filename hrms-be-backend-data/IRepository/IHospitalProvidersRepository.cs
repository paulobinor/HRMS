using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
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
        Task<HospitalProvidersDTO> GetHospitalProvidersByCompany(string ProvidersNames, int companyId);
        Task<IEnumerable<HodDTO>> GetAllHospitalProvidersCompanyId(long companyId);
    }
}
