using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public interface ICountryRepository
    {
        Task<List<CountryVm>> GetCountries();
        Task<List<CountryVm>> GetAllCountries();
    }
}
