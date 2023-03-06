using Com.XpressPayments.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.CountryStateLga
{
    public interface ILgaRepository
    {
         Task<IEnumerable<LgaDTO>> GetAllLga(int StateID);
        Task<LgaDTO> GetLgaByStateId(int StateID, int LGAID);
    }
}
