using System.Collections.Generic;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface IBranchService
    {
        Task<BaseResponse<CreateBranchDTO>> CreateBranch(CreateBranchDTO payload);
        Task<BaseResponse<List<BranchDTO>>> GetAllBranches(int CompanyID);
        Task<BaseResponse<DeleteBranchDTO>> DeleteBranch(DeleteBranchDTO payload);

        Task<BaseResponse<BranchDTO>> GetBranchByID(DeleteBranchDTO payload);
        Task<BaseResponse<UpdateBranchDTO>> UpdateBranch(UpdateBranchDTO payload);
    }
}