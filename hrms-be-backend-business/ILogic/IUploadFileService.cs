using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace hrms_be_backend_business.ILogic
{
    public interface IUploadFileService
    {
        Task<ExecutedResult<string>> UploadFile(IFormFile formFile, string FullName);
    }
   
}
