using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.ViewModel;
using System.Security.Claims;

namespace hrms_be_backend_business.ILogic
{
    public interface ICurrencyService
    {
        Task<ExecutedResult<string>> CreateCurrency(CurrencyCreateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<string>> UpdateCurrency(CurrencyUpdateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<IEnumerable<CurrencyVm>>> GetCurrencies(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<CurrencyVm>> GetCurrencyById(int Id, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
    }
}
