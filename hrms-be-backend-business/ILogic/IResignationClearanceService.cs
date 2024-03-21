using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;

namespace hrms_be_backend_business.ILogic
{
    public interface IResignationClearanceService
    {
        Task<ExecutedResult<string>> SubmitResignationClearance(ResignationClearanceDTO payload, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<ResignationClearanceDTO>> GetResignationClearanceByID(long ID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<ResignationClearanceDTO>> GetResignationClearanceByUserID(long UserID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<ResignationClearanceDTO>>> GetAllResignationClearanceByCompany(PaginationFilter filter, long companyID, string AccessKey, string RemoteIpAddress);


        //Task<BaseResponse> GetPendingResignationClearanceByUserID(long userID, string AccessKey, string RemoteIpAddress);
        //Task<BaseResponse> ApprovePendingResignationClearance(ApproveResignationClearanceDTO request, string AccessKey, string RemoteIpAddress);
        //Task<BaseResponse> DisapprovePendingResignationClearance(DisapprovePendingResignationClearanceDTO request, string AccessKey, string RemoteIpAddress;

    }
}
