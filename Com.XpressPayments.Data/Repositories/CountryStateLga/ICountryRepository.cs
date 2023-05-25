using Com.XpressPayments.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.CountryStateLga
{
    public interface ICountryRepository
    {
        Task<IEnumerable<CountryDTO>> GetAllCountries();
        Task<CountryDTO> GetCountryByName(string CountryName);
    }
}
