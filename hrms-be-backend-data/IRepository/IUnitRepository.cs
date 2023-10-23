using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public interface IUnitRepository
    {
        Task<string> ProcessUnit(ProcessUnitReq payload);
        Task<string> DeleteUnit(DeleteUnitReq payload);
        Task<UnitWithTotalVm> GetUnites(long CompanyId, int PageNumber, int RowsOfPage);
        Task<UnitWithTotalVm> GetUnitesDeleted(long CompanyId, int PageNumber, int RowsOfPage);
        Task<UnitVm> GetUnitById(long Id);
        Task<UnitVm> GetUnitByName(string UnitName, long CompanyId);
    }
}
