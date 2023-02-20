using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface ILgaService
    {
        Task<BaseResponse<List<LgaCodeDTO>>> GetAllLGAByState(int StateID, int LGAID);
        Task<BaseResponse<List<LgaCodeDTO>>> GetAllLGA(int StateID);
    }
}
