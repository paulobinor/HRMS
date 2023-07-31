using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Data.IRepository
{
    public interface IStateCodeRepo
    {
        Task<List<StateCodeDTO>> GetAllStateCountryID(int CountryID, int StateID);
        Task<List<StateCodeDTO>> GetAllState(int CountryID);
    }
}
