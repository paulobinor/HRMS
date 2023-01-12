using System.Collections.Generic;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Data.IRepository
{
    public interface IBankRepository
    {
        Task<dynamic> CreateBank(CreateBankDTO bankDTO);
        Task<dynamic> DeleteBank(DeleteBankDTO deleteBank);
        Task<IEnumerable<BanksDTO>> GetAllBank();
        Task<BanksDTO> GetBankByCBNCode(string BankCode);
        Task<BanksDTO> GetBankById(double bankID);
        Task<dynamic> UpdateBank(UpdateBankDTO bankDTO);
    }
}