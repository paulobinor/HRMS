using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface IStateService
    {
        Task<BaseResponse<List<StateCodeDTO>>> GetAllStateCountryID(int CountryID, int StateID);
        //Task<BaseResponse<List<StateCodeDTO>>> GetAllState();
        Task<BaseResponse<List<StateCodeDTO>>> GetAllState(int CountryID);
    }
}
