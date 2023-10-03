using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public interface IEarningsRepository
    {
        Task<string> ProcessEarnings(EarningsReq payload);
        Task<string> DeleteEarnings(EarningsDeleteReq payload);
        Task<EarningsVm> GetEarnings(long CompanyId);
        Task<EarningsVm> GetEarningsById(long Id);

        Task<string> ProcessEarningsComputation(EarningsComputationReq payload);
        Task<List<EarningsComputationVm>> GetEarningsComputation(long EarningsId);

        Task<string> ProcessEarningsItem(EarningItemReq payload);
        Task<string> DeleteEarningsItem(EarningsItemDeleteReq payload);
        Task<List<EarningsItemVm>> GetEarningsItem(long CompanyId);
        Task<EarningsItemVm> GetEarningsItemById(long Id);

        Task<EarningsCRAVm> GetEarningsCRA(long CompanyId);
    }
}
