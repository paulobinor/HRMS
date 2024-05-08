using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;

namespace hrms_be_backend_business.ILogic
{
    public interface IResignationService
    {
        Task<ExecutedResult<string>> SubmitResignation(ResignationRequestVM request, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<string>> UploadLetter(IFormFile signedResignationLetter, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<string>> UpdateResignation(UpdateResignationDTO updateDTO, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<ResignationDTO>> GetResignationByID(long ID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<ResignationDTO>>> GetResignationByEmployeeID(long UserID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<ResignationDTO>>> GetResignationByCompanyID(PaginationFilter filter, long companyID, string AccessKey, string RemoteIpAddress);

        //Task<ExecutedResult<IEnumerable<ResignationDTO>>> GetAllResignations(string AccessKey, string RemoteIpAddress);
        //Task<ExecutedResult<string>> DeleteResignation(DeleteResignationDTO request, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<ResignationDTO>>> GetPendingResignationByEmployeeID(long employeeID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<ResignationDTO>>> GetPendingResignationByCompanyID(long companyID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<string>> ApprovePendingResignation(ApprovePendingResignationDTO request, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<string>> DisapprovePendingResignation(DisapprovePendingResignation request, string AccessKey, string RemoteIpAddress);
        
    }
}
