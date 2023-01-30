using System.Collections.Generic;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface IBankService
    {
        Task<BaseResponse<CreateBankDTO>> CreateBank(CreateBankDTO payload);
        Task<BaseResponse<List<BanksDTO>>> GetAllBanks();
        Task<BaseResponse<BanksDTO>> GetBankByID(int bankID);
        Task<BaseResponse<UpdateBankDTO>> UpdateBank(UpdateBankDTO payload);
    }
}