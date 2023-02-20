using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Data.IRepository
{
    public interface ILgaCodeRepo
    {
        Task<List<LgaCodeDTO>> GetAllLGAbyStateId(int StateID, int LGAID);
        Task<List<LgaCodeDTO>> GetAllLGA(int StateID);
    }
}
