using System.Collections.Generic;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Data.IRepository
{
    public interface IBranchRepository
    {
        Task<int> CreateBranch(CreateBranchDTO payload);
        Task<int> DeleteBranch(DeleteBranchDTO payload);
        Task<List<BranchDTO>> GetAllBranches(int CompanyID);
        Task<BranchDTO> GetBranchByID(DeleteBranchDTO payload);
        Task<int> UpdateBranch(UpdateBranchDTO payload);
    }
}