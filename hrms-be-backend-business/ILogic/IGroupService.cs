using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;

namespace hrms_be_backend_business.ILogic
{
    public interface IGroupService
    {
        Task<BaseResponse> CreateGroup(CreateGroupDTO GroupDto, RequesterInfo requester);
        Task<BaseResponse> CreateGroupBulkUpload(IFormFile payload, RequesterInfo requester);
        Task<BaseResponse> UpdateGroup(UpdateGroupDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteGroup(DeleteGroupDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveGroup(RequesterInfo requester);
        Task<BaseResponse> GetAllGroup(RequesterInfo requester);
        Task<BaseResponse> GetGroupbyId(long GroupID, RequesterInfo requester);
        Task<BaseResponse> GetGroupbyCompanyId(long companyId, RequesterInfo requester);
    }
}
