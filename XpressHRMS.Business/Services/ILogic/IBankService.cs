using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface IBankService
    {
        Task<BaseResponse> CreateBank(CreateBankDTO payload);
        Task<BaseResponse> GetAllBanks();
        Task<BaseResponse> GetBankByID(int bankID);
        Task<BaseResponse> UpdateBank(UpdateBankDTO payload);
    }
}