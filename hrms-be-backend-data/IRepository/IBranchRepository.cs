using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public interface IBranchRepository
    {
        Task<string> ProcessBranch(ProcessBranchReq payload);
        Task<string> DeleteBranch(DeleteBranchReq payload);
        Task<BranchWithTotalVm> GetBranches(long CompanyId, int PageNumber, int RowsOfPage);
        Task<BranchWithTotalVm> GetBranchesDeleted(long CompanyId, int PageNumber, int RowsOfPage);
        Task<BranchVm> GetBranchById(long Id);
        Task<BranchVm> GetBranchByName(string BranchName, long CompanyId);
    }
}
