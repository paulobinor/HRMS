using Com.XpressPayments.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.Branch
{
    public interface IBranchRepository
    {
        Task<dynamic> CreateBranch(CreateBranchDTO branch, string createdbyUserEmail);
        Task<int> UpdateBranch(UpdateBranchDTO payload, string updatedbyUserEmail);
        Task<dynamic> DeleteBranch(DeleteBranchDTO Dept, string deletedbyUserEmail);
        Task<IEnumerable<BranchDTO>> GetAllActiveBranch();
        Task<IEnumerable<BranchDTO>> GetAllBranch();
        Task<BranchDTO> GetBranchById(long BranchID);
        Task<BranchDTO> GetBranchByName(string BranchName);
        Task<BranchDTO> GetBranchByCompany(string BranchName, int companyId);
        Task<IEnumerable<BranchDTO>> GetAllBranchbyCompanyId(long companyId);
        //public async Task<BranchDTO> GetBranchByCompanyId(long companyId, long BranchID);




    }
}
