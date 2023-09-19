using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public interface IEarningsRepository
    {
        Task<string> ProcessEarnings(EarningsReq payload);
        Task<string> DeleteEarnings(EarningsDeleteReq payload);
        Task<List<EarningsVm>> GetEarnings(long CompanyId);
        Task<EarningsVm> GetEarningsById(long Id);

        Task<string> ProcessEarningsItem(EarningItemReq payload);
        Task<string> DeleteEarningsItem(EarningsItemDeleteReq payload);
        Task<List<EarningsItemVm>> GetEarningsItem(long EarningsId);
    }
}
