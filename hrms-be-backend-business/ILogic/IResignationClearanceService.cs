using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;

namespace hrms_be_backend_business.ILogic
{
    public interface IResignationClearanceService
    {
        Task<BaseResponse> SubmitResignationClearance(RequesterInfo requesterInfo, ResignationClearanceVM payload);
        Task<BaseResponse> UploadItemsReturnedToDepartmant(IFormFile ItemsReturnedToDepartmant);
        Task<BaseResponse> GetResignationClearanceByID(long ID, RequesterInfo requester);
        Task<BaseResponse> GetResignationClearanceByUserID(long UserID, RequesterInfo requester);
        Task<BaseResponse> GetPendingResignationClearanceByUserID(RequesterInfo requester, long userID);
        Task<BaseResponse> ApprovePendingResignationClearance(ApproveResignationClearanceDTO request, RequesterInfo requester);
        Task<BaseResponse> DisapprovePendingResignationClearance(DisapprovePendingResignationClearanceDTO request, RequesterInfo requester);

    }
}
