using Com.XpressPayments.Data.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.CountryStateLga
{
    public interface ILgaRepository
    {
         Task<IEnumerable<LgaDTO>> GetAllLga(long StateID);
        Task<IEnumerable> GetLgaByStateId(long StateID);
        Task<LgaDTO> GetLgaByName(string LGA_Name);
    }
}
