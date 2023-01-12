using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface IBranchService
    {
        Task<BaseResponse> CreateBranch(CreateBranchDTO payload);
        Task<BaseResponse> DeleteBranch(DeleteBranchDTO payload);
        Task<BaseResponse> GetAllBranches(int CompanyID);
        Task<BaseResponse> GetBranchByID(DeleteBranchDTO payload);
        Task<BaseResponse> UpdateBranch(UpdateBranchDTO payload);
    }
}