using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.GenericResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.CountryStateLga
{
    public  interface IStateRepository
    {
        Task<IEnumerable<StateDTO>> GetAllState(int CountryID);
        Task<StateDTO> GetStateByCountryId(int CountryID, int StateID);
    }
}
