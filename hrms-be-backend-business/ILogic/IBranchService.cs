using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;

namespace hrms_be_backend_business.ILogic
{
    public interface IBranchService
    {
        Task<BaseResponse> CreateBranch(CreateBranchDTO BranchDto, RequesterInfo requester);
        Task<BaseResponse> CreateBranchBulkUpload(IFormFile payload, RequesterInfo requester);
        Task<BaseResponse> UpdateBranch(UpdateBranchDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteBranch(DeleteBranchDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveBranch(RequesterInfo requester);
        Task<BaseResponse> GetAllBranch(RequesterInfo requester);
        Task<BaseResponse> GetBranchbyId(long BranchID, RequesterInfo requester);
        Task<BaseResponse> GetBranchbyCompanyId(long companyId, RequesterInfo requester);



    }
}
