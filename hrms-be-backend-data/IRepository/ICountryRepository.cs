using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface ICountryRepository
    {
        Task<IEnumerable<CountryDTO>> GetAllCountries();
        Task<CountryDTO> GetCountryByName(string CountryName);
    }
}
