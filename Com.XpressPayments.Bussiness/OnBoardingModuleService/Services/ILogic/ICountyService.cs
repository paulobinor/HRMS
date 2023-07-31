using Com.XpressPayments.Data.GenericResponse;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.ILogic
{
    public  interface ICountyService
    {
        Task<BaseResponse> GetAllCountry(RequesterInfo requester);
    }
}
