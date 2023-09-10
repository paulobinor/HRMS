using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;

namespace hrms_be_backend_business.ILogic
{
    public interface IResignationService
    {
        Task<BaseResponse> SubmitResignation(RequesterInfo requesterInfo, ResignationRequestVM request);
        Task<BaseResponse> UploadLetter(IFormFile signedResignationLetter);
        Task<BaseResponse> GetResignationByID(long ID, RequesterInfo requester);
        Task<BaseResponse> GetResignationByUserID(long UserID, RequesterInfo requester);
        Task<BaseResponse> GetResignationByCompanyID(long companyID, bool isApproved, RequesterInfo requester);
        Task<BaseResponse> DeleteResignation(DeleteResignationDTO request, RequesterInfo requester);
        Task<BaseResponse> GetPendingResignationByUserID(RequesterInfo requester, long userID);
        Task<BaseResponse> ApprovePendingResignation(ApprovePendingResignationDTO request, RequesterInfo requester);
        Task<BaseResponse> DisapprovePendingResignation(DisapprovePendingResignation request, RequesterInfo requester);
        Task<BaseResponse> UpdateResignation(UpdateResignationDTO updateDTO, RequesterInfo requester);
    }
}
