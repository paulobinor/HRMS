using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public interface ICurrencyRepository
    {
        Task<string> ProcessCurrency(CurrencyReq payload);
        Task<List<CurrencyVm>> GetCurrencies();
        Task<List<CurrencyVm>> GetCurrencyById(int Id);
    }
}
