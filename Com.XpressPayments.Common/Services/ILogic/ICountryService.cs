using System.Collections.Generic;
using System.Threading.Tasks;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface ICountryService
    {
        Task<BaseResponse<List<CountryCode>>> GetAllCounytries();
    }
}
